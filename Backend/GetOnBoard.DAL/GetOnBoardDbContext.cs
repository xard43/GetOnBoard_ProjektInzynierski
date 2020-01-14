using GetOnBoard.Models;
using GetOnBoard.Models.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GetOnBoard.DAL
{
	public class GetOnBoardDbContext : IdentityDbContext<ApplicationUser>
	{
		private readonly IHostingEnvironment hostingEnvironment;

		public GetOnBoardDbContext(DbContextOptions<GetOnBoardDbContext> options, IHostingEnvironment hostingEnvironment) : base(options)
		{
			this.hostingEnvironment = hostingEnvironment;
		}

		public virtual DbSet<UserProfile> Profiles { get; set; }

		public virtual DbSet<GameSession> GameSessions { get; set; }

		public virtual DbSet<BoardGame> BoardGames { get; set; }

		public virtual DbSet<GameSessionApplicationUser> GameSessionApplicationUsers { get; set; }

		public virtual DbSet<UserProfile> UserProfiles { get; set; }

		public virtual DbSet<GameSessionBoardGame> GameSessionBoardGames { get; set; }

		public virtual DbSet<MessageEntity> MessageEntities { get; set; }

		public virtual DbSet<Contact> Contact { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			builder.Entity<ApplicationUser>().HasOne(u => u.Profile).WithOne(p => p.ApplicationUser).HasForeignKey<UserProfile>(p => p.ApplicationUserID);

			builder.Entity<GameSessionApplicationUser>().HasOne(gu => gu.GameSession).WithMany(gm => gm.Players).HasForeignKey(gu => gu.GameSessionID);
			builder.Entity<GameSessionApplicationUser>().HasOne(gu => gu.ApplicationUser).WithMany(gm => gm.GameSessions).HasForeignKey(gu => gu.ApplicationUserID);

			builder.Entity<GameSessionBoardGame>().HasOne(bg => bg.GameSession).WithMany(gs => gs.BoardGames).HasForeignKey(bg => bg.GameSessionID);
			builder.Entity<GameSessionBoardGame>().HasOne(gs => gs.BoardGame).WithMany(bg => bg.GameSessions).HasForeignKey(gs => gs.BoardGameID);

			builder.Entity<MessageEntity>().HasOne(ms => ms.GameSession).WithMany(gs => gs.Messages).HasForeignKey(ms => ms.GameSessionId);

			//#region GameSessions
			//builder.Entity<GameSession>().HasData(

			//	new { ID = 1, City = "Poznań", Address = "Poznanska 23", Name = "Chodz pograc", Description = " Jakiś opis, który mówi nam coś o wydarzeniu :D ", TimeStart = DateTime.Now, TimeEnd = DateTime.Now.AddHours(5), IsActive = true, IsCanceled = false, Slots = 5, SlotsFree = 1, UserAdminID = "04072b26-1d6e-4fcd-8d31-460de3754a47" },
			//	new { ID = 2, City = "Poznań", Address = "Poznanska 23", Name = "Chodz pograc2", Description = " Jakiś opis, który mówi nam coś o wydarzeniu :D ", TimeStart = DateTime.Now.AddDays(2), TimeEnd = DateTime.Now.AddDays(2).AddHours(3), IsActive = true, IsCanceled = false, Slots = 5, SlotsFree = 2, UserAdminID = "61b598e1-5e2e-41ad-a2e7-9e3badb02a4e" },
			//	new { ID = 3, City = "Warszawa", Address = "Poznanska 23/5", Name = "Chodz pograc3", Description = " Jakiś opis, który mówi nam coś o wydarzeniu :D ", TimeStart = DateTime.Now.AddDays(3), TimeEnd = DateTime.Now.AddDays(3).AddHours(5), IsActive = false, IsCanceled = true, Slots = 3, SlotsFree = 0, UserAdminID = "61b598e1-5e2e-41ad-a2e7-9e3badb02a4e" },
			//	new { ID = 4, City = "Gdańsk", Address = "Poznanska 23", Name = "Chodz pograc4", Description = " Jakiś opis, który mówi nam coś o wydarzeniu :D ", TimeStart = DateTime.Now.AddDays(2), TimeEnd = DateTime.Now.AddDays(2).AddHours(4), IsActive = false, IsCanceled = false, Slots = 5, SlotsFree = 0, UserAdminID = "61b598e1-5e2e-41ad-a2e7-9e3badb02a4e" },
			//	new { ID = 5, City = "Warszawa", Address = "Poznanska 23", Name = "Chodz pograc5", Description = " Jakiś opis, który mówi nam coś o wydarzeniu :D ", TimeStart = DateTime.Now.AddDays(2), TimeEnd = DateTime.Now.AddDays(2).AddHours(3), IsActive = true, IsCanceled = false, Slots = 6, SlotsFree = 3, UserAdminID = "04072b26-1d6e-4fcd-8d31-460de3754a47" },
			//	new { ID = 6, City = "Gdańsk", Address = "Poznanska 23", Name = "Chodz pograc6", Description = " Jakiś opis, który mówi nam coś o wydarzeniu :D ", TimeStart = DateTime.Now.AddDays(3), TimeEnd = DateTime.Now.AddDays(3).AddHours(6), IsActive = true, IsCanceled = false, Slots = 8, SlotsFree = 4, UserAdminID = "d65a392a-c2cf-4a9b-91f7-5491b62d69d5" },
			//	new { ID = 7, City = "Warszawa", Address = "Poznanska 23/2", Name = "Chodz pograc6", Description = " Jakiś opis, który mówi nam coś o wydarzeniu :D ", TimeStart = DateTime.Now.AddDays(4), TimeEnd = DateTime.Now.AddDays(4).AddHours(5), IsActive = false, IsCanceled = false, Slots = 7, SlotsFree = 1, UserAdminID = "d65a392a-c2cf-4a9b-91f7-5491b62d69d5" },
			//	new { ID = 8, City = "Kraków", Address = "Poznanska 23", Name = "Chodz pograc7", Description = " Jakiś opis, który mówi nam coś o wydarzeniu :D ", TimeStart = DateTime.Now.AddDays(4), TimeEnd = DateTime.Now.AddDays(4).AddHours(2), IsActive = true, IsCanceled = false, Slots = 4, SlotsFree = 2, UserAdminID = "d65a392a-c2cf-4a9b-91f7-5491b62d69d5" },
			//	new { ID = 9, City = "Gdańsk", Address = "Poznanska 23", Name = "Chodz pograc8", Description = " Jakiś opis, który mówi nam coś o wydarzeniu :D ", TimeStart = DateTime.Now.AddDays(5), TimeEnd = DateTime.Now.AddDays(5).AddHours(6), IsActive = false, IsCanceled = true, Slots = 3, SlotsFree = 0, UserAdminID = "1ad72b85-0d2d-4ffc-bbe6-a20521b1040f" },
			//	new { ID = 10, City = "Łódź", Address = "Poznanska 23", Name = "Chodz pograc9", Description = " Jakiś opis, który mówi nam coś o wydarzeniu :D ", TimeStart = DateTime.Now.AddDays(5), TimeEnd = DateTime.Now.AddDays(5).AddHours(3), IsActive = true, IsCanceled = false, Slots = 5, SlotsFree = 2, UserAdminID = "5d1bffc7-151c-4983-ae9a-32a3bbaa50e6" },

			//	new { ID = 11, City = "Łódź", Address = "Kozacka 23", Name = "Chodz pograc9", Description = " Jakiś opis, który mówi nam coś o wydarzeniu :D ", TimeStart = DateTime.Now.AddDays(3), TimeEnd = DateTime.Now.AddDays(3).AddHours(1), IsActive = false, IsCanceled = true, Slots = 7, SlotsFree = 3, UserAdminID = "ccdf68a3-c5ef-42a9-ad85-489b0595d755" },
			//	new { ID = 12, City = "Szczecin", Address = "Poznanska 23", Name = "Chodz pograc10", Description = " Jakiś opis, który mówi nam coś o wydarzeniu :D ", TimeStart = DateTime.Now.AddDays(4), TimeEnd = DateTime.Now.AddDays(4).AddHours(3), IsActive = true, IsCanceled = false, Slots = 6, SlotsFree = 3, UserAdminID = "1ad72b85-0d2d-4ffc-bbe6-a20521b1040f" },
			//	new { ID = 13, City = "Łódź", Address = "Kozacka 23", Name = "Chodz pograc11", Description = " Jakiś opis, który mówi nam coś o wydarzeniu :D ", TimeStart = DateTime.Now.AddDays(2), TimeEnd = DateTime.Now.AddDays(2).AddHours(8), IsActive = false, IsCanceled = true, Slots = 5, SlotsFree = 0, UserAdminID = "5670df40-a7db-417d-9aa0-126cd8d72d65" },

			//	new { ID = 14, City = "Szczecin", Address = "Kozacka 23", Name = "Chodz pograc48", Description = " Jakiś opis, który mówi nam coś o wydarzeniu :D ", TimeStart = DateTime.Now.AddDays(5), TimeEnd = DateTime.Now.AddDays(5).AddHours(3), IsActive = true, IsCanceled = false, Slots = 3, SlotsFree = 0, UserAdminID = "6291a29e-fdd7-4732-bb01-14802f6e4622" },
			//	new { ID = 15, City = "Łódź", Address = "Poznanska 23", Name = "Chodz pograc18", Description = " Jakiś opis, który mówi nam coś o wydarzeniu :D ", TimeStart = DateTime.Now.AddDays(6), TimeEnd = DateTime.Now.AddDays(6).AddHours(6), IsActive = true, IsCanceled = false, Slots = 8, SlotsFree = 4, UserAdminID = "279f1cb5-c8e4-4bd3-b66e-3a1b2b0adcfa" },
			//	new { ID = 16, City = "Szczecin", Address = "Kozacka 23", Name = "Chodz pograc28", Description = " Jakiś opis, który mówi nam coś o wydarzeniu :D ", TimeStart = DateTime.Now.AddDays(4), TimeEnd = DateTime.Now.AddDays(4).AddHours(4), IsActive = true, IsCanceled = false, Slots = 9, SlotsFree = 3, UserAdminID = "d65a392a-c2cf-4a9b-91f7-5491b62d69d5" },
			//	new { ID = 17, City = "Szczecin", Address = "Poznanska 23", Name = "Chodz pograc28", Description = " Jakiś opis, który mówi nam coś o wydarzeniu :D ", TimeStart = DateTime.Now.AddDays(7), TimeEnd = DateTime.Now.AddDays(7).AddHours(5), IsActive = false, IsCanceled = false, Slots = 14, SlotsFree = 12, UserAdminID = "5d1bffc7-151c-4983-ae9a-32a3bbaa50e6" },
			//	//wydarzenia bez graczy jeszcze
			//	new { ID = 18, City = "Warszawa", Address = "Poznanska 23", Name = "Chodz pograc28", Description = " Jakiś opis, który mówi nam coś o wydarzeniu :D ", TimeStart = DateTime.Now.AddDays(3), TimeEnd = DateTime.Now.AddDays(3).AddHours(1), IsActive = true, IsCanceled = false, Slots = 12, SlotsFree = 11, UserAdminID = "5d1bffc7-151c-4983-ae9a-32a3bbaa50e6" },
			//	new { ID = 19, City = "Gdańsk", Address = "Poznanska 23", Name = "Chodz pograc28", Description = " Jakiś opis, który mówi nam coś o wydarzeniu :D ", TimeStart = DateTime.Now.AddDays(4), TimeEnd = DateTime.Now.AddDays(4).AddHours(5), IsActive = true, IsCanceled = false, Slots = 11, SlotsFree = 10, UserAdminID = "1c77ab70-2b1e-4a75-a06b-0e2c1a6d0d39" },
			//	new { ID = 20, City = "Łódź", Address = "Poznanska 23", Name = "Chodz pograc28", Description = " Jakiś opis, który mówi nam coś o wydarzeniu :D ", TimeStart = DateTime.Now.AddDays(5), TimeEnd = DateTime.Now.AddDays(5).AddHours(4), IsActive = true, IsCanceled = false, Slots = 10, SlotsFree = 9, UserAdminID = "5670df40-a7db-417d-9aa0-126cd8d72d65" },
			//	new { ID = 21, City = "Poznań", Address = "Poznanska 23", Name = "Chodz pograc28", Description = " Jakiś opis, który mówi nam coś o wydarzeniu :D ", TimeStart = DateTime.Now.AddDays(6), TimeEnd = DateTime.Now.AddDays(6).AddHours(2), IsActive = true, IsCanceled = false, Slots = 13, SlotsFree = 12, UserAdminID = "5d1bffc7-151c-4983-ae9a-32a3bbaa50e6" },
			//	new { ID = 22, City = "Kraków", Address = "Poznanska 23", Name = "Chodz pograc28", Description = " Jakiś opis, który mówi nam coś o wydarzeniu :D ", TimeStart = DateTime.Now.AddDays(2), TimeEnd = DateTime.Now.AddDays(2).AddHours(5), IsActive = true, IsCanceled = false, Slots = 12, SlotsFree = 11, UserAdminID = "a3f4cc1c-982d-4855-b250-583e747b24b8" },
			//	new { ID = 23, City = "Szczecin", Address = "Poznanska 23", Name = "Chodz pograc28", Description = " Jakiś opis, który mówi nam coś o wydarzeniu :D ", TimeStart = DateTime.Now.AddDays(1), TimeEnd = DateTime.Now.AddDays(1).AddHours(3), IsActive = false, IsCanceled = true, Slots = 14, SlotsFree = 13, UserAdminID = "d1542f38-f391-47df-a6f5-47cbad5ba31d" }

			//	);
			//#endregion
			//#region add users do events
			//builder.Entity<GameSessionApplicationUser>().HasData(
			//	//04072b26-1d6e-4fcd-8d31-460de3754a47 - admin // 4/5 ilość slotów
			//	new { ID = 1, GameSessionID = 1, ApplicationUserID = "1c77ab70-2b1e-4a75-a06b-0e2c1a6d0d39" },
			//	new { ID = 2, GameSessionID = 1, ApplicationUserID = "5d1bffc7-151c-4983-ae9a-32a3bbaa50e6" },
			//	new { ID = 3, GameSessionID = 1, ApplicationUserID = "905dfb6f-baed-4eb0-aafb-79e6ab961f5a" },
			//	//61b598e1-5e2e-41ad-a2e7-9e3badb02a4e // 3/5
			//	new { ID = 4, GameSessionID = 2, ApplicationUserID = "905dfb6f-baed-4eb0-aafb-79e6ab961f5a" },
			//	new { ID = 5, GameSessionID = 2, ApplicationUserID = "2128e2be-fb1b-4119-bf4f-ec27ac14a969" },
			//	//61b598e1-5e2e-41ad-a2e7-9e3badb02a4e // 3/3
			//	new { ID = 6, GameSessionID = 3, ApplicationUserID = "4fc795f6-4902-40b7-b0c6-5a3300ab62b7" },
			//	new { ID = 7, GameSessionID = 3, ApplicationUserID = "5b242cf9-77d4-4a62-85c1-2cd984604b75" },
			//	//61b598e1-5e2e-41ad-a2e7-9e3badb02a4e // 5/5
			//	new { ID = 8, GameSessionID = 4, ApplicationUserID = "a3f4cc1c-982d-4855-b250-583e747b24b8" },
			//	new { ID = 9, GameSessionID = 4, ApplicationUserID = "ccdf68a3-c5ef-42a9-ad85-489b0595d755" },
			//	new { ID = 10, GameSessionID = 4, ApplicationUserID = "f242ea83-9bc8-4346-a2de-2252c3597353" },
			//	new { ID = 11, GameSessionID = 4, ApplicationUserID = "4fc795f6-4902-40b7-b0c6-5a3300ab62b7" },
			//	//04072b26-1d6e-4fcd-8d31-460de3754a47 // 3/6
			//	new { ID = 12, GameSessionID = 5, ApplicationUserID = "4fc795f6-4902-40b7-b0c6-5a3300ab62b7" },
			//	new { ID = 13, GameSessionID = 5, ApplicationUserID = "5b242cf9-77d4-4a62-85c1-2cd984604b75" },
			//	//d65a392a-c2cf-4a9b-91f7-5491b62d69d5 // 4/8
			//	new { ID = 14, GameSessionID = 6, ApplicationUserID = "b074fc5d-3286-438d-83e6-55be443637fd" },
			//	new { ID = 15, GameSessionID = 6, ApplicationUserID = "a3f4cc1c-982d-4855-b250-583e747b24b8" },
			//	new { ID = 16, GameSessionID = 6, ApplicationUserID = "3d5e6b3a-11f1-4d7a-bc21-1c3ffbaa8825" },
			//	//d65a392a-c2cf-4a9b-91f7-5491b62d69d5 // 6/7
			//	new { ID = 17, GameSessionID = 7, ApplicationUserID = "73c9ef61-169c-4981-a395-4844170e7403" },
			//	new { ID = 18, GameSessionID = 7, ApplicationUserID = "2128e2be-fb1b-4119-bf4f-ec27ac14a969" },
			//	new { ID = 19, GameSessionID = 7, ApplicationUserID = "3d5e6b3a-11f1-4d7a-bc21-1c3ffbaa8825" },
			//	new { ID = 20, GameSessionID = 7, ApplicationUserID = "3302a5c8-489f-47f4-98ea-6e41601f3b85" },
			//	new { ID = 21, GameSessionID = 7, ApplicationUserID = "4fc795f6-4902-40b7-b0c6-5a3300ab62b7" },
			//	//d65a392a-c2cf-4a9b-91f7-5491b62d69d5 // 2/4
			//	new { ID = 22, GameSessionID = 8, ApplicationUserID = "3302a5c8-489f-47f4-98ea-6e41601f3b85" },
			//	//1ad72b85-0d2d-4ffc-bbe6-a20521b1040f // 3/3
			//	new { ID = 23, GameSessionID = 9, ApplicationUserID = "f631ac26-dc92-444c-81a0-e0a05d381f2b" },
			//	new { ID = 24, GameSessionID = 9, ApplicationUserID = "f82b149f-c85e-4e39-9395-d775ffa994a5" },
			//	//5d1bffc7-151c-4983-ae9a-32a3bbaa50e6 // 3/5
			//	new { ID = 25, GameSessionID = 10, ApplicationUserID = "3302a5c8-489f-47f4-98ea-6e41601f3b85" },
			//	new { ID = 26, GameSessionID = 10, ApplicationUserID = "093e79ec-36f6-4563-984e-e0c1fa7f92d0" },
			//	//ccdf68a3-c5ef-42a9-ad85-489b0595d755 // 4/7
			//	new { ID = 27, GameSessionID = 11, ApplicationUserID = "d1542f38-f391-47df-a6f5-47cbad5ba31d" },
			//	new { ID = 28, GameSessionID = 11, ApplicationUserID = "093e79ec-36f6-4563-984e-e0c1fa7f92d0" },
			//	new { ID = 29, GameSessionID = 11, ApplicationUserID = "1ad72b85-0d2d-4ffc-bbe6-a20521b1040f" },
			//	//1ad72b85-0d2d-4ffc-bbe6-a20521b1040f // 3/6
			//	new { ID = 30, GameSessionID = 12, ApplicationUserID = "5f1e3c74-bcd7-4dbf-a1f8-f62f74543994" },
			//	new { ID = 31, GameSessionID = 12, ApplicationUserID = "ce4615db-b45a-4b18-8d9f-89cbf71c4d7a" },
			//	//5670df40-a7db-417d-9aa0-126cd8d72d65 // 5/5
			//	new { ID = 32, GameSessionID = 13, ApplicationUserID = "4fc795f6-4902-40b7-b0c6-5a3300ab62b7" },
			//	new { ID = 33, GameSessionID = 13, ApplicationUserID = "d1542f38-f391-47df-a6f5-47cbad5ba31d" },
			//	new { ID = 34, GameSessionID = 13, ApplicationUserID = "093e79ec-36f6-4563-984e-e0c1fa7f92d0" },
			//	new { ID = 35, GameSessionID = 13, ApplicationUserID = "04072b26-1d6e-4fcd-8d31-460de3754a47" },
			//	//6291a29e-fdd7-4732-bb01-14802f6e4622 // 3/3
			//	new { ID = 36, GameSessionID = 14, ApplicationUserID = "1ad72b85-0d2d-4ffc-bbe6-a20521b1040f" },
			//	new { ID = 37, GameSessionID = 14, ApplicationUserID = "279f1cb5-c8e4-4bd3-b66e-3a1b2b0adcfa" },
			//	//279f1cb5-c8e4-4bd3-b66e-3a1b2b0adcfa // 4/8
			//	new { ID = 38, GameSessionID = 15, ApplicationUserID = "905dfb6f-baed-4eb0-aafb-79e6ab961f5a" },
			//	new { ID = 39, GameSessionID = 15, ApplicationUserID = "b074fc5d-3286-438d-83e6-55be443637fd" },
			//	new { ID = 40, GameSessionID = 15, ApplicationUserID = "ccdf68a3-c5ef-42a9-ad85-489b0595d755" },
			//	//d65a392a-c2cf-4a9b-91f7-5491b62d69d5 // 6/9
			//	new { ID = 41, GameSessionID = 16, ApplicationUserID = "9fa0ddc2-2194-4f4f-93ca-de17d3ad2de0" },
			//	new { ID = 42, GameSessionID = 16, ApplicationUserID = "6bc13e6e-bdd4-41a2-8e33-8dbd1a28e396" },
			//	new { ID = 43, GameSessionID = 16, ApplicationUserID = "d1542f38-f391-47df-a6f5-47cbad5ba31d" },
			//	new { ID = 44, GameSessionID = 16, ApplicationUserID = "1c77ab70-2b1e-4a75-a06b-0e2c1a6d0d39" },
			//	new { ID = 45, GameSessionID = 16, ApplicationUserID = "3302a5c8-489f-47f4-98ea-6e41601f3b85" },
			//	//5d1bffc7-151c-4983-ae9a-32a3bbaa50e6 // 2/14
			//	new { ID = 46, GameSessionID = 17, ApplicationUserID = "3302a5c8-489f-47f4-98ea-6e41601f3b85" }


			//	);
			//#endregion
			//#region boardgames data
			//builder.Entity<BoardGame>().HasData(

			//	new { ID = 1, Name = "Monopoly 1", Description = "Opis gry, który zachęci użytkowników 1", PlayersMin = 2, PlayersMax = 5, Category = "Kategoria 1", Score = 5, Level = 2 },
			//	new { ID = 2, Name = "Monopoly 2", Description = "Opis gry, który zachęci użytkowników 2", PlayersMin = 3, PlayersMax = 9, Category = "Kategoria 2", Score = 3, Level = 4 },
			//	new { ID = 3, Name = "Monopoly 3", Description = "Opis gry, który zachęci użytkowników 3", PlayersMin = 5, PlayersMax = 8, Category = "Kategoria 3", Score = 6, Level = 6 },
			//	new { ID = 4, Name = "Monopoly 4", Description = "Opis gry, który zachęci użytkowników 4", PlayersMin = 3, PlayersMax = 5, Category = "Kategoria 4", Score = 7, Level = 8 },
			//	new { ID = 5, Name = "Monopoly 5", Description = "Opis gry, który zachęci użytkowników 5", PlayersMin = 4, PlayersMax = 7, Category = "Kategoria 5", Score = 2, Level = 9 },
			//	new { ID = 6, Name = "Monopoly 6", Description = "Opis gry, który zachęci użytkowników 6", PlayersMin = 2, PlayersMax = 6, Category = "Kategoria 6", Score = 1, Level = 1 },
			//	new { ID = 7, Name = "Monopoly 7", Description = "Opis gry, który zachęci użytkowników 7", PlayersMin = 3, PlayersMax = 9, Category = "Kategoria 7", Score = 8, Level = 2 },
			//	new { ID = 8, Name = "Monopoly 8", Description = "Opis gry, który zachęci użytkowników 8", PlayersMin = 5, PlayersMax = 12, Category = "Kategoria 8", Score = 9, Level = 5 },
			//	new { ID = 9, Name = "Monopoly 9", Description = "Opis gry, który zachęci użytkowników 9", PlayersMin = 2, PlayersMax = 13, Category = "Kategoria 9", Score = 15, Level = 6 },
			//	new { ID = 10, Name = "Monopoly 10", Description = "Opis gry, który zachęci użytkowników 10", PlayersMin = 3, PlayersMax = 15, Category = "Kategoria 10", Score = 4, Level = 3 },

			//	new { ID = 11, Name = "Pędzące ślimaki 11", Description = "Inny Opis Gry ", PlayersMin = 6, PlayersMax = 11, Category = "Przygodowa 1", Score = 4, Level = 6 },
			//	new { ID = 12, Name = "Pędzące ślimaki 12", Description = "Inny Opis Gry ", PlayersMin = 2, PlayersMax = 7, Category = "Przygodowa 1", Score = 2, Level = 2 },
			//	new { ID = 13, Name = "Pędzące ślimaki 13", Description = "Inny Opis Gry ", PlayersMin = 1, PlayersMax = 8, Category = "Przygodowa 1", Score = 4, Level = 5 },
			//	new { ID = 14, Name = "Pędzące ślimaki 14", Description = "Inny Opis Gry ", PlayersMin = 1, PlayersMax = 9, Category = "Przygodowa 1", Score = 7, Level = 8 },
			//	new { ID = 15, Name = "Pędzące ślimaki 15", Description = "Inny Opis Gry ", PlayersMin = 1, PlayersMax = 4, Category = "Przygodowa 1", Score = 1, Level = 9 },
			//	new { ID = 16, Name = "Pędzące ślimaki 16", Description = "Inny Opis Gry ", PlayersMin = 6, PlayersMax = 7, Category = "Przygodowa 1", Score = 9, Level = 1 },
			//	new { ID = 17, Name = "Pędzące ślimaki 17", Description = "Inny Opis Gry ", PlayersMin = 7, PlayersMax = 9, Category = "Przygodowa 1", Score = 2, Level = 3 },
			//	new { ID = 18, Name = "Pędzące ślimaki 18", Description = "Inny Opis Gry ", PlayersMin = 2, PlayersMax = 5, Category = "Przygodowa 1", Score = 3, Level = 2 },
			//	new { ID = 19, Name = "Pędzące ślimaki 19", Description = "Inny Opis Gry ", PlayersMin = 5, PlayersMax = 8, Category = "Przygodowa 1", Score = 7, Level = 6 },
			//	new { ID = 20, Name = "Pędzące ślimaki 20", Description = "Inny Opis Gry ", PlayersMin = 7, PlayersMax = 7, Category = "Przygodowa 1", Score = 7, Level = 5 },

			//	new { ID = 21, Name = "50 sekund 1", Description = "Super fajna gra dla najlepszych", PlayersMin = 2, PlayersMax = 7, Category = "Strzelanka ", Score = 2, Level = 1 },
			//	new { ID = 22, Name = "50 sekund 2", Description = "Super fajna gra dla najlepszych", PlayersMin = 3, PlayersMax = 8, Category = "Strzelanka ", Score = 4, Level = 4 },
			//	new { ID = 23, Name = "50 sekund 3", Description = "Super fajna gra dla najlepszych", PlayersMin = 1, PlayersMax = 9, Category = "Strzelanka ", Score = 6, Level = 2 },
			//	new { ID = 24, Name = "50 sekund 4", Description = "Super fajna gra dla najlepszych", PlayersMin = 6, PlayersMax = 6, Category = "Strzelanka ", Score = 6, Level = 7 },
			//	new { ID = 25, Name = "50 sekund 5", Description = "Super fajna gra dla najlepszych", PlayersMin = 5, PlayersMax = 7, Category = "Strzelanka ", Score = 3, Level = 5 },
			//	new { ID = 26, Name = "50 sekund 6", Description = "Super fajna gra dla najlepszych", PlayersMin = 3, PlayersMax = 4, Category = "Strzelanka ", Score = 7, Level = 6 },
			//	new { ID = 27, Name = "50 sekund 7", Description = "Super fajna gra dla najlepszych", PlayersMin = 6, PlayersMax = 7, Category = "Strzelanka ", Score = 2, Level = 5 },
			//	new { ID = 28, Name = "50 sekund 8", Description = "Super fajna gra dla najlepszych", PlayersMin = 3, PlayersMax = 5, Category = "Strzelanka ", Score = 1, Level = 4 },
			//	new { ID = 29, Name = "50 sekund 9", Description = "Super fajna gra dla najlepszych", PlayersMin = 2, PlayersMax = 3, Category = "Strzelanka ", Score = 7, Level = 5 },
			//	new { ID = 30, Name = "50 sekund 10", Description = "Super fajna gra dla najlepszych", PlayersMin = 4, PlayersMax = 6, Category = "Strzelanka ", Score = 8, Level = 3 },

			//	new { ID = 31, Name = "Masuj dzika 1", Description = "Jeszcze lepsza super fajna gra dla najlepszych", PlayersMin = 1, PlayersMax = 6, Category = "Ostra jazda bez trzymanki", Score = 4, Level = 3 },
			//	new { ID = 32, Name = "Masuj dzika 2", Description = "Jeszcze lepsza super fajna gra dla najlepszych", PlayersMin = 4, PlayersMax = 6, Category = "Ostra jazda bez trzymanki", Score = 2, Level = 4 },
			//	new { ID = 33, Name = "Masuj dzika 3", Description = "Jeszcze lepsza super fajna gra dla najlepszych", PlayersMin = 3, PlayersMax = 8, Category = "Ostra jazda bez trzymanki", Score = 3, Level = 1 },
			//	new { ID = 34, Name = "Masuj dzika 4", Description = "Jeszcze lepsza super fajna gra dla najlepszych", PlayersMin = 4, PlayersMax = 11, Category = "Ostra jazda bez trzymanki", Score = 1, Level = 6 },
			//	new { ID = 35, Name = "Masuj dzika 5", Description = "Jeszcze lepsza super fajna gra dla najlepszych", PlayersMin = 6, PlayersMax = 15, Category = "Ostra jazda bez trzymanki", Score = 2, Level = 4 },
			//	new { ID = 36, Name = "Masuj dzika 6", Description = "Jeszcze lepsza super fajna gra dla najlepszych", PlayersMin = 3, PlayersMax = 7, Category = "Ostra jazda bez trzymanki", Score = 3, Level = 5 },
			//	new { ID = 37, Name = "Masuj dzika 7", Description = "Jeszcze lepsza super fajna gra dla najlepszych", PlayersMin = 5, PlayersMax = 9, Category = "Ostra jazda bez trzymanki", Score = 7, Level = 3 },
			//	new { ID = 38, Name = "Masuj dzika 8", Description = "Jeszcze lepsza super fajna gra dla najlepszych", PlayersMin = 8, PlayersMax = 10, Category = "Ostra jazda bez trzymanki", Score = 9, Level = 4 },
			//	new { ID = 39, Name = "Masuj dzika 9", Description = "Jeszcze lepsza super fajna gra dla najlepszych", PlayersMin = 2, PlayersMax = 8, Category = "Ostra jazda bez trzymanki", Score = 2, Level = 8 },
			//	new { ID = 40, Name = "Masuj dzika 10", Description = "Jeszcze lepsza super fajna gra dla najlepszych", PlayersMin = 4, PlayersMax = 9, Category = "Ostra jazda bez trzymanki", Score = 3, Level = 9 },

			//	new { ID = 41, Name = "Potrzyj gołębia1", Description = "Najlepsza z najlepszych gra", PlayersMin = 3, PlayersMax = 8, Category = "Strategiczna", Score = 3, Level = 9 },
			//	new { ID = 42, Name = "Potrzyj gołębia2", Description = "Najlepsza z najlepszych gra", PlayersMin = 5, PlayersMax = 8, Category = "Strategiczna", Score = 4, Level = 2 },
			//	new { ID = 43, Name = "Potrzyj gołębia3", Description = "Najlepsza z najlepszych gra", PlayersMin = 2, PlayersMax = 4, Category = "Strategiczna", Score = 6, Level = 1 },
			//	new { ID = 44, Name = "Potrzyj gołębia4", Description = "Najlepsza z najlepszych gra", PlayersMin = 1, PlayersMax = 2, Category = "Strategiczna", Score = 1, Level = 5 },
			//	new { ID = 45, Name = "Potrzyj gołębia5", Description = "Najlepsza z najlepszych gra", PlayersMin = 3, PlayersMax = 11, Category = "Strategiczna", Score = 6, Level = 7 },
			//	new { ID = 46, Name = "Potrzyj gołębia6", Description = "Najlepsza z najlepszych gra", PlayersMin = 5, PlayersMax = 15, Category = "Strategiczna", Score = 2, Level = 5 },
			//	new { ID = 47, Name = "Potrzyj gołębia7", Description = "Najlepsza z najlepszych gra", PlayersMin = 4, PlayersMax = 5, Category = "Strategiczna", Score = 6, Level = 6 },
			//	new { ID = 48, Name = "Potrzyj gołębia8", Description = "Najlepsza z najlepszych gra", PlayersMin = 7, PlayersMax = 8, Category = "Strategiczna", Score = 4, Level = 5 },
			//	new { ID = 49, Name = "Potrzyj gołębia9", Description = "Najlepsza z najlepszych gra", PlayersMin = 6, PlayersMax = 12, Category = "Strategiczna", Score = 8, Level = 3 },
			//	new { ID = 50, Name = "Potrzyj gołębia10", Description = "Najlepsza z najlepszych gra", PlayersMin = 8, PlayersMax = 16, Category = "Strategiczna", Score = 2, Level = 1 }

			//	);
			//#endregion
			//#region gamesInGameSessions
			//builder.Entity<GameSessionBoardGame>().HasData(
			//	//04072b26-1d6e-4fcd-8d31-460de3754a47 - admin // 4/5 ilość slotów
			//	new { ID = 1, GameSessionID = 1, BoardGameID = 2 },
			//	new { ID = 2, GameSessionID = 1, BoardGameID = 4 },
			//	new { ID = 3, GameSessionID = 1, BoardGameID = 5 },
			//	//61b598e1-5e2e-41ad-a2e7-9e3badb02a4e // 3/5
			//	new { ID = 4, GameSessionID = 2, BoardGameID = 11 },
			//	new { ID = 5, GameSessionID = 2, BoardGameID = 22 },
			//	//61b598e1-5e2e-41ad-a2e7-9e3badb02a4e // 3/3
			//	new { ID = 6, GameSessionID = 3, BoardGameID = 5 },
			//	new { ID = 7, GameSessionID = 3, BoardGameID = 16 },
			//	//61b598e1-5e2e-41ad-a2e7-9e3badb02a4e // 5/5
			//	new { ID = 8, GameSessionID = 4, BoardGameID = 28 },
			//	new { ID = 9, GameSessionID = 4, BoardGameID = 25 },
			//	new { ID = 10, GameSessionID = 4, BoardGameID = 33 },
			//	new { ID = 11, GameSessionID = 4, BoardGameID = 37 },
			//	//04072b26-1d6e-4fcd-8d31-460de3754a47 // 3/6
			//	new { ID = 12, GameSessionID = 5, BoardGameID = 50 },
			//	new { ID = 13, GameSessionID = 5, BoardGameID = 43 },
			//	//d65a392a-c2cf-4a9b-91f7-5491b62d69d5 // 4/8
			//	new { ID = 14, GameSessionID = 6, BoardGameID = 29 },
			//	new { ID = 15, GameSessionID = 6, BoardGameID = 2 },
			//	new { ID = 16, GameSessionID = 6, BoardGameID = 7 },
			//	//d65a392a-c2cf-4a9b-91f7-5491b62d69d5 // 6/7
			//	new { ID = 17, GameSessionID = 7, BoardGameID = 8 },
			//	new { ID = 18, GameSessionID = 7, BoardGameID = 5 },
			//	new { ID = 19, GameSessionID = 7, BoardGameID = 7 },
			//	new { ID = 20, GameSessionID = 7, BoardGameID = 17 },
			//	new { ID = 21, GameSessionID = 7, BoardGameID = 19 },
			//	//d65a392a-c2cf-4a9b-91f7-5491b62d69d5 // 2/4
			//	new { ID = 22, GameSessionID = 8, BoardGameID = 11 },
			//	//1ad72b85-0d2d-4ffc-bbe6-a20521b1040f // 3/3
			//	new { ID = 23, GameSessionID = 9, BoardGameID = 15 },
			//	new { ID = 24, GameSessionID = 9, BoardGameID = 16 },
			//	//5d1bffc7-151c-4983-ae9a-32a3bbaa50e6 // 3/5
			//	new { ID = 25, GameSessionID = 10, BoardGameID = 32 },
			//	new { ID = 26, GameSessionID = 10, BoardGameID = 37 },
			//	//ccdf68a3-c5ef-42a9-ad85-489b0595d755 // 4/7
			//	new { ID = 27, GameSessionID = 11, BoardGameID = 35 },
			//	new { ID = 28, GameSessionID = 11, BoardGameID = 30 },
			//	new { ID = 29, GameSessionID = 11, BoardGameID = 19 },
			//	//1ad72b85-0d2d-4ffc-bbe6-a20521b1040f // 3/6
			//	new { ID = 30, GameSessionID = 12, BoardGameID = 22 },
			//	new { ID = 31, GameSessionID = 12, BoardGameID = 34 },
			//	//5670df40-a7db-417d-9aa0-126cd8d72d65 // 5/5
			//	new { ID = 32, GameSessionID = 13, BoardGameID = 21 },
			//	new { ID = 33, GameSessionID = 13, BoardGameID = 4 },
			//	new { ID = 34, GameSessionID = 13, BoardGameID = 6 },
			//	new { ID = 35, GameSessionID = 13, BoardGameID = 8 },
			//	//6291a29e-fdd7-4732-bb01-14802f6e4622 // 3/3
			//	new { ID = 36, GameSessionID = 14, BoardGameID = 5 },
			//	new { ID = 37, GameSessionID = 14, BoardGameID = 49 },
			//	//279f1cb5-c8e4-4bd3-b66e-3a1b2b0adcfa // 4/8
			//	new { ID = 38, GameSessionID = 15, BoardGameID = 50 },
			//	new { ID = 39, GameSessionID = 15, BoardGameID = 44 },
			//	new { ID = 40, GameSessionID = 15, BoardGameID = 21 },
			//	//d65a392a-c2cf-4a9b-91f7-5491b62d69d5 // 6/9
			//	new { ID = 41, GameSessionID = 16, BoardGameID = 34 },
			//	new { ID = 42, GameSessionID = 16, BoardGameID = 22 },
			//	new { ID = 43, GameSessionID = 16, BoardGameID = 11 },
			//	new { ID = 44, GameSessionID = 16, BoardGameID = 12 },
			//	new { ID = 45, GameSessionID = 16, BoardGameID = 35 },
			//	//5d1bffc7-151c-4983-ae9a-32a3bbaa50e6 // 2/14
			//	new { ID = 46, GameSessionID = 17, BoardGameID = 4 }


			//	);
			//#endregion

		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				IConfigurationRoot configuration = new ConfigurationBuilder()
				   .SetBasePath(hostingEnvironment.ContentRootPath)
				   .AddJsonFile("appsettings.json")
				   .Build();
				var connectionString = configuration.GetConnectionString("DefaultConnection");
				optionsBuilder.UseSqlServer(connectionString);
			}
		}
	}
}
