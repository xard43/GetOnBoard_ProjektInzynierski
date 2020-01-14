using System;
using System.Net;

namespace GetOnBoard.Logging
{
	public class RequestException : Exception
	{
		public HttpStatusCode StatusCode { get; set; }
		public string ExceptionMessage { get; set; }
		public int EventID { get; set; }

		public RequestException(HttpStatusCode statusCode, string exceptionMessage)
		{
			StatusCode = statusCode;
			ExceptionMessage = exceptionMessage;
			EventID = LoggingEvents.GenericError;
		}
		public RequestException(HttpStatusCode statusCode, string exceptionMessage, int eventID)
		{
			StatusCode = statusCode;
			ExceptionMessage = exceptionMessage;
			EventID = eventID;
		}
	}
}
