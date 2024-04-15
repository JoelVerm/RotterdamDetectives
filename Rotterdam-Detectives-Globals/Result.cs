using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RotterdamDetectives_Globals
{
    public class Result<TValue, TError>
    {
        public readonly TValue? Value;
        public readonly TError? Error;
        public readonly bool IsOk;

        protected Result(bool ok, TValue? value, TError? error)
        {
            IsOk = ok;
            Value = value;
            Error = error;
        }

        public static implicit operator Result<TValue, TError>(TValue value) => Ok(value);

        public static Result<TValue, TError> Ok(TValue value) => new Result<TValue, TError>(true, value, default);
        public static Result<TValue, TError> Err(TError error) => new Result<TValue, TError>(false, default, error);
    }

    public class Result<TValue> : Result<TValue, string>
    {
        public Result(bool ok, TValue? value, string? error) : base(ok, value, error) { }

        public static implicit operator Result<TValue>(TValue value) => Ok(value);

        public static new Result<TValue> Ok(TValue value) => new Result<TValue>(true, value, null);
        public static new Result<TValue> Err(string error) => new Result<TValue>(false, default, error);
    }

    public class Result : Result<bool?>
    {
        public Result(bool ok, string? error) : base(ok, null, error) { }

        public static Result Ok() => new Result(true, null);
        public static new Result Err(string error) => new Result(false, error);
    }
}
