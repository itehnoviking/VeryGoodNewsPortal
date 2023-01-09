using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeryGoodNewsPortal.Core.Interfaces;
using VeryGoodNewsPortal.Core.Interfaces.Data;
using VeryGoodNewsPortal.Data.Entities;

namespace VeryGoodNewsPortal.Domain.Services
{
    public class RoleService : IRoleService
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public RoleService(IMapper mapper,
            IUnitOfWork unitOfWork,
            IConfiguration configuration)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }


        public async Task<Guid> CreateRole(string name)
        {
            var id = Guid.NewGuid();

            await _unitOfWork.Roles.AddAsync(new Role()
            {
                Id = id,
                Name = name
            });

            await _unitOfWork.Commit();

            return id;
        }

        public async Task<Guid> GetRoleIdByNameAsync(string name)
        {
            var id = await (await _unitOfWork.Roles
                .FindBy(role => role.Name.Equals(name)))
                .Select(role => role.Id)
                .FirstOrDefaultAsync();

            return id;
        }

        public async Task<string> GetRoleNameByIdAsync(Guid roleId)
        {
            var role = await _unitOfWork.Roles.GetByIdAsync(roleId);

            return role.Name;
        }
    }
}
