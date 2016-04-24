using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace WeatherApp.Web.Models
{
	public class ErrorResult : IHttpActionResult
	{
		public string Message { get; set; }

		public HttpRequestMessage Request { get; set; }
		public HttpStatusCode StatusCode { get; set; }

		public ErrorResult(string message, HttpRequestMessage request, HttpStatusCode statusCode)
		{
			Message = message;
			Request = request;
			StatusCode = statusCode;
		}

		public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
		{
			var response =  Request.CreateErrorResponse(StatusCode, Message);

			return Task.FromResult(response);
		}
	}
}