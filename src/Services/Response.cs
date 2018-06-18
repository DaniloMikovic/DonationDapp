using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DonacijeDapp.Services
{
    public class Response<T>
    {
        public T Value { get; set; }
        public string Message { get; set; }
        public bool Succeeded { get; set; }
    }
}
