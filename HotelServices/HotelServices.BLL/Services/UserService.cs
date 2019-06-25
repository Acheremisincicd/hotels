using AutoMapper;
using HotelServices.BLL.DTO;
using HotelServices.BLL.Infrastructure;
using HotelServices.BLL.Interfaces;
using HotelServices.DAL.EF;
using HotelServices.DAL.Entities;
using HotelServices.DAL.Interfaces;
using Microsoft.AspNet.Identity;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HotelServices.BLL.Services
{
    /// <summary>
    /// Class for working with users from database using IUnitOfWork 
    /// </summary>
    public class UserService : IUserService
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        IUnitOfWork Database { get; set; }

        /// <summary>
        /// Constuctor which gets IUnitOfWork; with help of it we interact with the DAL level
        /// </summary>
        /// <param name="uow"></param>
        public UserService(IUnitOfWork uow)
        {
            Database = uow;
        }

        /// <summary>
        /// Adding a new user to the database
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        public async Task<OperationDetails> Create(UserDTO userDto)
        {
            ApplicationUser user = await Database.UserManager.FindByEmailAsync(userDto.Email);
            if (user == null)
            {
                user = new ApplicationUser {
                    Email = userDto.Email,
                    UserName = userDto.Email
                };
                var result = await Database.UserManager.CreateAsync(user, userDto.Password);
                if (result.Errors.Count() > 0)
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
                //adding a role
                await Database.UserManager.AddToRoleAsync(user.Id, userDto.Role);
                //creating a user profile
                ClientProfile clientProfile = new ClientProfile { Id = user.Id, Address = userDto.Address, ClientName = userDto.ClientName };
                Database.ClientManager.Create(clientProfile);
                await Database.SaveAsync();
                logger.Log(LogLevel.Info, "Новый пользователь с id = " + user.Id + " и почтой "+ user.Email + " добавлен " +
                    "в базу данных. ");
                return new OperationDetails(true, "Регистрация успешно пройдена", "");
            }
            else
            {
                return new OperationDetails(false, "Пользователь с таким логином уже существует", "Email");
            }
        }

        /// <summary>
        /// User authentication in the system
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        public async Task<ClaimsIdentity> Authenticate(UserDTO userDto)
        {
            ClaimsIdentity claim = null;
            //finding the user
            ApplicationUser user = await Database.UserManager.FindAsync(userDto.Email, userDto.Password);
            //authorizing him and returning the ClaimsIdentity object
            if (user != null)
                claim = await Database.UserManager.CreateIdentityAsync(user,DefaultAuthenticationTypes.ApplicationCookie);
            logger.Log(LogLevel.Info, "Пользователь " + userDto.Email + " авторизован в системе.");
            return claim;
        }

        /// <summary>
        /// Initial initialization of the database
        /// </summary>
        /// <param name="adminDto"></param>
        /// <param name="managerDto"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        public async Task SetInitialData(UserDTO adminDto, UserDTO managerDto, List<string> roles)
        {
            foreach (string roleName in roles)
            {
                var role = await Database.RoleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    role = new ApplicationRole { Name = roleName };
                    await Database.RoleManager.CreateAsync(role);
                }
            }
            await Create(adminDto);
            await Create(managerDto);
        }

        /// <summary>
        /// Getting a user from the database for further work with it
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserDTO GetUser(string id)
        {
            ClientProfile client = Database.ClientManager.GetClient(id);
            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.CreateMap<ClientProfile, UserDTO>()
                                .ForMember("Id", opt => opt.MapFrom(src => src.Id))
                                .ForMember("Email", opt => opt.MapFrom(src => src.ApplicationUser.Email))
                                .ForMember("ClientName", opt => opt.MapFrom(src => src.ClientName))
                                .ForMember("Address", opt => opt.MapFrom(src => src.Address))
                                .ForMember("UserName", opt => opt.MapFrom(src => src.ApplicationUser.UserName))
                                .ForMember("СashAccount", opt => opt.MapFrom(src => src.СashAccount))
                                );
            logger.Log(LogLevel.Info, "Получен пользователь с id = " + id + ".");
            return Mapper.Map<ClientProfile, UserDTO>(client);
        }

        /// <summary>
        /// Updating of the existing user in the database
        /// </summary>
        /// <param name="userDTO"></param>
        public void UpdateClient(UserDTO userDTO)
        {
            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.CreateMap<UserDTO, ClientProfile>()
                                .ForMember("Id", opt => opt.MapFrom(src => src.Id))
                                .ForMember("ClientName", opt => opt.MapFrom(src => src.ClientName))
                                .ForMember("Address", opt => opt.MapFrom(src => src.Address))
                                .ForMember("СashAccount", opt => opt.MapFrom(src => src.СashAccount))
                                );
            ClientProfile client = Mapper.Map<UserDTO, ClientProfile>(userDTO);
            Database.ClientManager.Update(client);
            logger.Log(LogLevel.Info, "Пользователь с id = " + client.Id + " обновлен в базе данных. ");
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
