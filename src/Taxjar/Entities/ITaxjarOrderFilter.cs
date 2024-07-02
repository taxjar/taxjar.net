using System;

namespace Taxjar
{
    public interface ITaxjarOrderFilter 
    {
        DateTime? TransactionDate { get; set; }

        DateTime? FromTransactionDate { get; set; }

        DateTime? ToTransactionDate { get; set; }

        string? Provider { get; set; }
    }
}
