using System;
using System.Collections.Generic;
using System.Linq;
using ge_repository.Models;
using ge_repository.DAL;
using ge_repository.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ge_repository.DAL { 
    
    public class ge_eventDAL : ige_eventDAL {
        public ge_DbContext context {get;set;}
        public ge_event message {get;set;}
        public ge_eventDAL(ge_DbContext Context) {
            context = Context;
        }
        public IEnumerable<ge_event> getUserEvents(string userId) {return null;}
        public ge_event addEvent(string UserId, string Message,string Return_URL, logLEVEL Level)
          {
          message = new ge_event();
          message.createdId = UserId;
          message.createdDT = DateTime.UtcNow;
          message.message = Message;
          message.returnUrl = Return_URL;
          message.level = Level;
          context.ge_event.Add(message);
          return message;
      }
        private bool ge_eventExists(Guid? Id)
        {
            if (Id == null) {
                return false;
            } else {
                return context.ge_event.Any(e => e.Id == Id.Value);
            }
        }
      public void Save(){
        context.Attach(message).State = EntityState.Modified;
        context.SaveChanges();

      }

      public void Dispose(){}
    }
}

