using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace GetOnBoard.Logging
{
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger _logger;

		public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
		{
			_logger = logger;
			_next = next;
		}

		//public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
		//{
		//	app.UseMiddleware<ExceptionMiddleware>();
		//}

		public async Task InvokeAsync(HttpContext httpContext)
		{
			try
			{
				await _next(httpContext);
			}
			catch (RequestException ex)
			{
				_logger.LogError(ex.EventID, $"Request Exception: {ex.ExceptionMessage} \n {ex.StackTrace}");
				await HandleRequestExceptionAsync(httpContext, ex);
			}
			catch (Exception ex)
			{
				_logger.LogError($"Something went wrong: {ex} \n {ex.StackTrace}");
				await HandleExceptionAsync(httpContext, ex);
			}
		}

		private static Task HandleRequestExceptionAsync(HttpContext context, RequestException ex)
		{
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)ex.StatusCode;
			return context.Response.WriteAsync(new ErrorDetails()
			{
				StatusCode = context.Response.StatusCode,
				Message = ex.ExceptionMessage
			}.ToString());
		}

		private static Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

			return context.Response.WriteAsync(new ErrorDetails()
			{
				StatusCode = context.Response.StatusCode,
				Message = $"Internal Server Error from the custom middleware. Message: {exception.Message}"
			}.ToString());
		}
	}
}
