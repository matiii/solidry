using System;
using System.Data;

namespace Solidry.Aspects
{
    public abstract class WithUnitOfWork
    {
        private readonly string _connectionString;
        private readonly IsolationLevel _isolationLevel;

        private SqlCommand _command;

        /// <summary>
        /// Create unit of work with read committed isolation level
        /// </summary>
        /// <param name="connectionString"></param>
        protected WithUnitOfWork(string connectionString) : this(connectionString, IsolationLevel.ReadCommitted)
        {
        }

        protected WithUnitOfWork(string connectionString, IsolationLevel isolationLevel)
        {
            _connectionString = connectionString;
            _isolationLevel = isolationLevel;
        }

        /// <summary>
        /// Catch unit of work exception
        /// </summary>
        /// <param name="exception"></param>
        /// <returns>If false will rethrow after rollback.</returns>
        protected abstract bool OnCatch(Exception exception);

        /// <summary>
        /// Catch rollback exception
        /// </summary>
        /// <param name="exception"></param>
        protected virtual void OnRollbackException(Exception exception)
        {

        }

        /// <summary>
        /// Execute script with parameters
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected int ExecuteNonQuery(string sql, params SqlParameter[] parameters)
        {
            _command.CommandText = sql;

            _command.Parameters.Clear();
            _command.Parameters.AddRange(parameters);

            return _command.ExecuteNonQuery();
        }

        /// <summary>
        /// Execute query with parameters
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected SqlDataReader Query(string sql, params SqlParameter[] parameters)
        {
            _command.CommandText = sql;

            _command.Parameters.Clear();
            _command.Parameters.AddRange(parameters);

            return _command.ExecuteReader();
        }

        /// <summary>
        /// Execute unit of work
        /// </summary>
        protected void Execute()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                _command = connection.CreateCommand();

                SqlTransaction transaction = connection.BeginTransaction(_isolationLevel);

                _command.Connection = connection;
                _command.Transaction = transaction;

                try
                {
                    //Execute work

                    // Attempt to commit the transaction.
                    transaction.Commit();
                }
                catch (Exception commitException)
                {
                    // Attempt to roll back the transaction.
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception rollbackExcpetion)
                    {
                        OnRollbackException(rollbackExcpetion);
                    }

                    if (!OnCatch(commitException))
                    {
                        throw;
                    }
                }
            }
        }
    }
}