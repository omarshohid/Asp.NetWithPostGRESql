
using System;
using System.Security.Principal;
using DAL.Models;
using DAL.Model;

namespace DAL.Repository
{
    public class UnitOfWork : IDisposable
    {
        private DatabaseContext context = new DatabaseContext();

        private GenericRepository<users> users;
        private GenericRepository<AspNetUsers> AspNetUsers;



        //======================custom repository====================//
        private SPRepository spRepository;
        public SPRepository SPRepository
        {
            get
            {

                if (this.spRepository == null)
                {
                    this.spRepository = new SPRepository(context);
                }
                return spRepository;
            }
        }
        //====================end of custom repository==================//

       
        //======================custom repository====================//
        private CustomRepository customRepository;
        public CustomRepository CustomRepository
        {
            get
            {

                if (this.customRepository == null)
                {
                    this.customRepository = new CustomRepository(context);
                }
                return customRepository;
            }
        }
        //====================end of custom repository==================//


        //=====================Generic Repositories =====================//

        public GenericRepository<users> usersRepository
        {
            get
            {

                if (this.users == null)
                {
                    this.users = new GenericRepository<users>(context);
                }
                return users;
            }
        }

        public GenericRepository<AspNetUsers> AspNetUsersRepository
        {
            get
            {

                if (this.AspNetUsers == null)
                {
                    this.AspNetUsers = new GenericRepository<AspNetUsers>(context);
                }
                return AspNetUsers;
            }
        }



        //==============================end===============================//


        public void Save()
        {
            try
            {
                context.SaveChanges();
            } catch(Exception ex)
            {
                
                throw ex;
            }
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}