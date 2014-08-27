using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QifApi.Transactions
{
    /// <summary>
    /// Used to describe an invalid transaction.
    /// </summary>
    public class InvalidTransactionException : Exception
    {
        private TransactionBase _Transaction;

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidTransactionException"/> class.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        public InvalidTransactionException(TransactionBase transaction)
            : this(string.Format("Invalid bank transaction: {0}", transaction), transaction)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidTransactionException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="transaction">The transaction.</param>
        public InvalidTransactionException(string message, TransactionBase transaction)
            : this(message, null, transaction)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidTransactionException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="transaction">The transaction.</param>
        public InvalidTransactionException(string message, Exception innerException, TransactionBase transaction)
            : base(message, innerException)
        {
            _Transaction = transaction;
        }

        /// <summary>
        /// Gets the transaction.
        /// </summary>
        /// <value>The transaction.</value>
        public TransactionBase Transaction
        {
            get
            {
                return _Transaction;
            }
        }
    }
}
