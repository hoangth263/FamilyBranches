﻿using InteractiveFamilyTree.DAO.Repositories;
using InteractiveFamilyTree.DTO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractiveFamilyTree.DAO.IRepositories;

public interface IFamilyMemberRepo : IGenericRepository<FamilyMember, int>
{
}
