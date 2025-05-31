using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Business.Utilities.Results
{
    public class ErrorDataResult<T> : IDataResult<T>
    {
        public bool Success => false;
        public string Message { get; }
        public T Data { get; }

        public ErrorDataResult(string message)
        {
            Data = default;
            Message = message;
        }

        public ErrorDataResult(T data, string message)
        {
            Data = data;
            Message = message;
        }
    }
}
