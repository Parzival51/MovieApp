using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Business.Utilities.Results
{
    public class SuccessDataResult<T> : IDataResult<T>
    {
        public bool Success => true;
        public string Message { get; }
        public T Data { get; }

        public SuccessDataResult(T data, string message = null)
        {
            Data = data;
            Message = message;
        }
    }
}
