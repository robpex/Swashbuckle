﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Http;

namespace Swashbuckle.Dummy.Controllers
{
    public class ParamsFromUriController : ApiController
    {
        [HttpGet]
        public decimal CalculateTax([FromUri]Address address, [FromUri]Transaction transaction)
        {
            throw new NotImplementedException();
        }

        [HttpHead]
        public bool SupportsCurrencies([FromUri]IEnumerable<string> currencies)
        {
            throw new NotImplementedException();
        }
    }

    public class Address
    {
        public string City { get; set; }

        public string State { get; set; }

        public string Zip { get; set; }
    }

    public class Transaction
    {
        public string Currency { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public TransactionType TransType { get; set; }
    }

    public enum TransactionType
    {
        TRANSFER,
        NETWORK
    }
}
