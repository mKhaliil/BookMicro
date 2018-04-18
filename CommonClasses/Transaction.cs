using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


    public class Transaction
    {
        public string WithdrawId { get; set; }
        public string TransactionId { get; set; }
        public string TransactionType { get; set; }
        public string TransactionAmount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string WithdrawAmount { get; set; }
        public DateTime WithdrawDate { get; set; }
        public string Status { get; set; }
        public string RowId { get; set; }
    }
