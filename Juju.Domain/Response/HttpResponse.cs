using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Juju.Domain.Response
{
    public class HttpResponse<T>
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public List<string> Errors { get; set; }
        public object ExtraData { get; set; }

        // Propiedades de paginación
        public int? TotalRecords { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public int? TotalPages => PageSize.HasValue && PageSize > 0 && TotalRecords.HasValue
            ? (int)Math.Ceiling((double)TotalRecords.Value / PageSize.Value)
            : null;

        public HttpResponse() { }

        public HttpResponse(HttpStatusCode statusCode, string message)
        {
            this.HttpStatusCode = statusCode;
            this.Message = message;
        }

        public HttpResponse(HttpStatusCode statusCode, string message, T data)
        {
            this.HttpStatusCode = statusCode;
            this.Message = message;
            this.Data = data;
        }

        public HttpResponse(HttpStatusCode statusCode, string message, List<string> errors)
        {
            this.HttpStatusCode = statusCode;
            this.Message = message;
            this.Errors = errors;
        }

        // Constructor para incluir paginación
        public HttpResponse(HttpStatusCode statusCode, string message, T data, int totalRecords, int pageNumber, int pageSize)
        {
            this.HttpStatusCode = statusCode;
            this.Message = message;
            this.Data = data;
            this.TotalRecords = totalRecords;
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }

        public HttpResponse(HttpStatusCode statusCode, string message, object extraData)
        {
            this.HttpStatusCode = statusCode;
            this.Message = message;
            this.ExtraData = extraData;
        }

        public static HttpResponse<T> Success(HttpStatusCode code, string message, T data) =>
            new() { Data = data, HttpStatusCode = code, Message = message };

        public static HttpResponse<T> Fail(HttpStatusCode code, string message) =>
            new() { HttpStatusCode = code, Message = message };
    }
}
