using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Utility
{
   public class ApiResponse<T>
    {
        public bool Status {  get; set; }

        public T? Data { get; set; } = default!;

        public List<string> Error { get; set; } = default!;

        public string Messages { get; set; } = default!;

        public static ApiResponse<T> Success(T? data, string messages="success")
        {
            return new ApiResponse<T>
            {
                Status = true,
                Data = data,
                Messages=messages,
            };
        }

        //Compile error Optional parameters must appear after all required parameters.
        public static ApiResponse<T> Failure( List<string> errors, string message = "failure")
        {
            return new ApiResponse<T>
            {
                Status = false,
                Error = errors,
            };
        }
    }
}
