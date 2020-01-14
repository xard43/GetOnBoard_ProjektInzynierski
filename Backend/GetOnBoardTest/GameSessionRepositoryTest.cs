using GetOnBoard.Controllers;
using GetOnBoard.DAL;
using GetOnBoard.DAL.Repositories;
using GetOnBoard.Logging;
using GetOnBoard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Hosting;

namespace GetOnBoardTest
{
	public class GameSessionRepositoryTest
	{
		private DbContextOptions<GetOnBoardDbContext> _options;
		private IHostingEnvironment _hostingEnvironment;
		public GameSessionRepositoryTest()
		{
			_options = new DbContextOptionsBuilder<GetOnBoardDbContext>()
				.UseInMemoryDatabase(databaseName: "GameSessionRepositoryTest")
				.Options;
			_hostingEnvironment = new HostingEnvironment();
		}
		//[Fact]
		//public async Task GameSessionRepository_ReturnGameSessionList_WhenExecuted()
		//{
		//	using (GetOnBoardDbContext dbContext = new GetOnBoardDbContext(_options, _hostingEnvironment))
		//	{
		//		//Arrange
		//		Mock<ILogger<GameSessionsRepository>> mockedLogger = new Mock<ILogger<GameSessionsRepository>>();

		//		dbContext.GameSessions.Add(new GameSession()
		//										{	ID = 1,
		//											IsActive = true,
		//											IsCanceled = false
		//										});
		//		dbContext.SaveChanges();
		//		var repository = new GameSessionsRepository(mockedLogger.Object, dbContext);

		//		//Act
		//		var eventList = await repository.GetGameSessionListAsync();

		//		//Assert

		//		Assert.Single(eventList);
		//		Assert.Equal(1, eventList.Where(it => it.ID == 1).FirstOrDefault().ID);
		//	}
		//}
		//public async Task GetGameSession_ReturnsBadRequest_WhenGameSessionNotFound()
		//{
		//Arrange
		//var mockedRepostory = new Mock<IGameSessionsRepository>();
		//mockedRepostory.Setup(x => x.GetGameSessionByIdAsync(It.IsAny<int>())).ReturnsAsync(() => null);
		//Mock<ILogger<GameSessionsController>> mockedLogger = new Mock<ILogger<GameSessionsController>>();

		//var controller = new GameSessionsController(mockedRepostory.Object, mockedLogger.Object);

		////Act
		//var ex = Record.ExceptionAsync(async () => { await controller.GetGameSession(0); });

		////Assert
		//Assert.IsType<RequestException>(await ex);
		//Assert.Equal(HttpStatusCode.BadRequest, ((RequestException)await ex).StatusCode);
		//}
		//[Fact]
		//public void GetSingleGameSession_ThrowsException_When
	}
}
