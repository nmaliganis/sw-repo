﻿using System;
using System.Linq;
using sw.auth.contracts.ContractRepositories;
using sw.auth.model.Companies;
using sw.auth.model.Departments;
using sw.auth.model.Members;
using sw.auth.model.Users;
using sw.auth.repository.Repositories.Base;
using sw.infrastructure.Domain.Queries;
using sw.infrastructure.Paging;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;

namespace sw.auth.repository.Repositories
{
    public class UserRepository : RepositoryBase<User, long>, IUserRepository
    {
        public UserRepository(ISession session)
            : base(session)
        {
        }

        public QueryResult<User> FindAllActiveUsersPagedOf(int? pageNum, int? pageSize)
        {
            var query = Session.QueryOver<User>();

            if (pageNum == -1 & pageSize == -1)
            {
                return new QueryResult<User>(query
                    .WhereNot(Restrictions.On<User>(c => c.Login).IsLike("su%"))
                    .JoinQueryOver<Member>(u => u.Member)
                    .WhereNot(Restrictions.On<Member>(c => c.Email).IsLike("su@%"))
                    .Where(u => u.Active == true)
                    .List().AsQueryable());
            }

            return new QueryResult<User>(query
                        .WhereNot(Restrictions.On<User>(c => c.Login).IsLike("su%"))
                        .JoinQueryOver<Member>(u => u.Member)
                        .WhereNot(Restrictions.On<Member>(c => c.Email).IsLike("su@%"))
                        .Where(u => u.Active == true)
                        .Skip(ResultsPagingUtility.CalculateStartIndex((int)pageNum, (int)pageSize))
                        .Take((int)pageSize).List().AsQueryable(),
                    query.ToRowCountQuery().RowCount(),
                    (int)pageSize)
                ;
        }

        public QueryResult<User> FindAllActiveUsersByCompanyPagedOf(long companyId, int? pageNum, int? pageSize)
        {
            var query = Session.QueryOver<User>();

            if (pageNum == -1 & pageSize == -1)
            {
                return new QueryResult<User>(query
                    .WhereNot(Restrictions.On<User>(c => c.Login).IsLike("su%"))
                    .JoinQueryOver<Member>(u => u.Member)
                    .WhereNot(Restrictions.On<Member>(c => c.Email).IsLike("su@%"))
                    .Where(m => m.Active == true)
                    .JoinQueryOver<MemberDepartment>(m => m.Departments)
                    .JoinQueryOver<Department>(md => md.Department)
                    .Where(d => d.Company.Id == companyId)
                    .And(d => d.Active == true)
                    .And(d => d.Company.Active == true)
                    .List().AsQueryable());
            }

            return new QueryResult<User>(query
                        .WhereNot(Restrictions.On<User>(c => c.Login).IsLike("su%"))
                        .JoinQueryOver<Member>(u => u.Member)
                        .WhereNot(Restrictions.On<Member>(c => c.Email).IsLike("su@%"))
                        .Where(m => m.Active == true)
                        .JoinQueryOver<MemberDepartment>(m => m.Departments)
                        .JoinQueryOver<Department>(md => md.Department)
                        .Where(d => d.Company.Id == companyId)
                        .And(d => d.Active == true)
                        .And(d => d.Company.Active == true)
                        .Skip(ResultsPagingUtility.CalculateStartIndex((int)pageNum, (int)pageSize))
                        .Take((int)pageSize).List().AsQueryable(),
                    query.ToRowCountQuery().RowCount(),
                    (int)pageSize)
                ;
        }

        public QueryResult<User> FindAllActiveUsersByDepartmentPagedOf(long departmentId, int? pageNum, int? pageSize)
        {
            var query = Session.QueryOver<User>();

            if (pageNum == -1 & pageSize == -1)
            {
                return new QueryResult<User>(query
                    .WhereNot(Restrictions.On<User>(c => c.Login).IsLike("su%"))
                    .JoinQueryOver<Member>(u => u.Member)
                    .WhereNot(Restrictions.On<Member>(c => c.Email).IsLike("su@%"))
                    .Where(m => m.Active == true)
                    .JoinQueryOver<MemberDepartment>(m => m.Departments)
                    .Where(md => md.Department.Id == departmentId)
                    .And(md => md.Active == true)
                    .List().AsQueryable());
            }

            return new QueryResult<User>(query
                        .WhereNot(Restrictions.On<User>(c => c.Login).IsLike("su%"))
                        .JoinQueryOver<Member>(u => u.Member)
                        .WhereNot(Restrictions.On<Member>(c => c.Email).IsLike("su@%"))
                        .Where(m => m.Active == true)
                        .JoinQueryOver<MemberDepartment>(m => m.Departments)
                        .Where(md => md.Department.Id == departmentId)
                        .And(md => md.Active == true)
                        .Skip(ResultsPagingUtility.CalculateStartIndex((int)pageNum, (int)pageSize))
                        .Take((int)pageSize).List().AsQueryable(),
                    query.ToRowCountQuery().RowCount(),
                    (int)pageSize)
                ;
        }

        public User FindUserByLoginAndEmail(string login, string email)
        {
            return (User)
                Session.CreateCriteria(typeof(User))
                    .CreateAlias("Member", "m", JoinType.InnerJoin)
                    .Add(Restrictions.Or(
                        Restrictions.Eq("Login", login),
                        Restrictions.Eq("m.Email", email)))
                    .Add(Expression.Eq("Active", true))
                    .Add(Expression.Eq("m.Active", true))
                    .SetCacheable(true)
                    .SetCacheMode(CacheMode.Normal)
                    .UniqueResult()
                ;

        }

        public User FindUserByLogin(string login)
        {
            return (User)
                Session.CreateCriteria(typeof(User))
                    .CreateAlias("Member", "m", JoinType.InnerJoin)
                    .Add(Restrictions.Or(
                        Restrictions.Eq("Login", login),
                        Restrictions.Eq("m.Email", login)))
                    .Add(Expression.Eq("Active", true))
                    .Add(Expression.Eq("m.Active", true))
                    .SetCacheable(true)
                    .SetCacheMode(CacheMode.Normal)
                    .UniqueResult()
                ;
        }

        public User FindUserByEmail(string email)
        {
            return (User)
                Session.CreateCriteria(typeof(User))
                    .CreateAlias("Member", "m", JoinType.InnerJoin)
                    .Add(Restrictions.Eq("m.Email", email))
                    .Add(Expression.Eq("Active", true))
                    .Add(Expression.Eq("m.Active", true))
                    .SetCacheable(true)
                    .SetCacheMode(CacheMode.Normal)
                    .UniqueResult()
                ;
        }

        public User FindUserByLoginAndPasswordAsync(string login, string password)
        {
            return
                (User)
                Session.CreateCriteria(typeof(User))
                    .Add(Expression.Eq("Login", login))
                    .Add(Expression.Eq("PasswordHash", password))
                    .Add(Expression.Eq("Active", true))
                    .SetCacheable(true)
                    .SetCacheMode(CacheMode.Normal)
                    .UniqueResult()
                ;
        }

        public User FindByUserIdAndActivationKey(long userIdToBeActivated, Guid activationKey)
        {
            return
                (User)
                Session.CreateCriteria(typeof(User))
                    .Add(Expression.Eq("Id", userIdToBeActivated))
                    .Add(Expression.Eq("ActivationKey", activationKey))
                    .SetCacheable(true)
                    .SetCacheMode(CacheMode.Normal)
                    .UniqueResult()
                ;
        }

        public User FindUserByRefreshTokenAsync(Guid refreshToken)
        {
            return (User)
                Session.CreateCriteria(typeof(User))
                    .CreateAlias("UserTokens", "t", JoinType.InnerJoin)
                    .Add(Expression.Eq("Active", true))
                    .Add(Expression.Eq("t.RefreshToken", refreshToken))
                    .Add(Expression.Eq("t.Expired", false))
                    .Add(Expression.Eq("t.Active", true))
                    .SetCacheable(true)
                    .SetCacheMode(CacheMode.Normal)
                    .UniqueResult()
                ;
        }
    }
}