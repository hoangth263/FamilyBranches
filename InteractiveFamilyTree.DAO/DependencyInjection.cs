using InteractiveFamilyTree.DAO.IRepositories;
using InteractiveFamilyTree.DAO.IServices;
using InteractiveFamilyTree.DAO.Repositories;
using InteractiveFamilyTree.DAO.Services;
using InteractiveFamilyTree.DTO.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractiveFamilyTree.DAO;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<InteractiveFamilyTreeOfficalContext>(option =>
        option.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));


        #region entity
        services.AddTransient<IInteractiveFamilyTreeOfficalContext, InteractiveFamilyTreeOfficalContext>();

        services.AddTransient<ICareerRepo, CareerRepo>();
        services.AddTransient<IChildAndParentsRelationShipRepo, ChildAndParentsRelationShipRepo>();
        services.AddTransient<ICoupleRelationshipRepo, CoupleRelationshipRepo>();
        services.AddTransient<IEventParticipantRepo, EventParticipantRepo>();
        services.AddTransient<IFamilyEventRepo, FamilyEventRepo>();
        services.AddTransient<IFamilyMemberRepo, FamilyMemberRepo>();
        services.AddTransient<IFamilyTreeRepo, FamilyTreeRepo>();
        services.AddTransient<IMemberRepo, MemberRepo>();

        services.AddTransient<ICareerService, CareerService>();
        services.AddTransient<IChildAndParentsRelationShipService, ChildAndParentsRelationShipService>();
        services.AddTransient<ICoupleRelationshipService, CoupleRelationshipService>();
        services.AddTransient<IEventParticipantService, EventParticipantService>();
        services.AddTransient<IFamilyEventService, FamilyEventService>();
        services.AddTransient<IFamilyMemberService, FamilyMemberService>();
        services.AddTransient<IFamilyTreeService, FamilyTreeService>();
        services.AddTransient<IMemberService, MemberService>();

        #endregion
        return services;
    }
}