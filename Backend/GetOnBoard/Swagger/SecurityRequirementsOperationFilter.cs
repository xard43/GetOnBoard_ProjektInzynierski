
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GetOnBoard.Swagger
{
	public class SecurityRequirementsOperationFilter : IOperationFilter
	{
		public void Apply(Swashbuckle.AspNetCore.Swagger.Operation operation, OperationFilterContext context)
		{
			if (!context.MethodInfo.GetCustomAttributes(true)
				  .Any(_ => _ is AllowAnonymousAttribute))
			{
				operation.Security = new List<IDictionary<string, IEnumerable<string>>>
									{
										new Dictionary<string, IEnumerable<string>>
											{
												{"Bearer", Array.Empty<string>()}
											}
									};
			}
		}
	}
}
