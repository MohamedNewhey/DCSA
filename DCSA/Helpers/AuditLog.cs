using DCSA.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace DCSA.Database
{
    public partial class DefaultConnection
    {
        public override int SaveChanges()
        {
            try
            {
                var identity = (ClaimsIdentity)HttpContext.Current.User.Identity;
                IEnumerable<Claim> claims = identity.Claims;
                int UserID = -1;
                try
                {
                    if(claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier) != null)
                         UserID = int.Parse(claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
                }
                catch
                {

                }

                var controller = HttpContext.Current.Request.RequestContext.RouteData.Values["Controller"].ToString();
                var action = HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString();
                foreach (var ent in this.ChangeTracker.Entries().Where(p => p.State == EntityState.Added || p.State == EntityState.Deleted || p.State == EntityState.Modified))
                {
                    // For each changed record, get the audit record entries and add them
                    foreach (AuditLogger x in GetAuditRecordsForChange(ent, UserID, controller, action))
                    {
                        this.AuditLoggers.Add(x);
                    }
                }

                return base.SaveChanges();
            }
            catch (Exception exx)
            {
                throw new Exception("Failed");
            }
        }
        private List<AuditLogger> GetAuditRecordsForChange(DbEntityEntry dbEntry, int userId, string controller, string action)
        {
            List<AuditLogger> result = new List<AuditLogger>();

            DateTime changeTime = DateTime.Now;

            // Get the Table() attribute, if one exists
            TableAttribute tableAttr = dbEntry.Entity.GetType().GetCustomAttributes(typeof(TableAttribute), false).SingleOrDefault() as TableAttribute;
            var temp = dbEntry.Entity.GetType().GetProperties();
            // Get table name (if it has a Table attribute, use that, otherwise get the pluralized name)
            string tableName = ObjectContext.GetObjectType(dbEntry.Entity.GetType()).Name;

            // Get primary key value (If you have more than one key column, this will need to be adjusted)
            //if (tableName.ToLower() == "actionlogger" && tableName.ToLower() == "suggestions")
            //{
            //    // Otherwise, don't do anything, we don't care about Unchanged or Detached entities
            //}
            //else
            //{
                if (dbEntry.State == EntityState.Added)
                {
                    // For Inserts, just add the whole record
                    // If the entity implements IDescribableEntity, use the description from Describe(), otherwise use ToString()
                    result.Add(new AuditLogger()
                    {
                        EventType = "Added", // Added
                        TableName = tableName,
                        RecordID = int.Parse(dbEntry.CurrentValues.GetValue<object>("ID").ToString()),  // Again, adjust this if you have a multi-column key
                                                                                                        //  ColumnName = propertyName,          // Or make it nullable, whatever you want
                        NewValue = GlobalHelper.EntityToString(dbEntry),
                        UserID = userId,
                        Time = changeTime,
                        Action = action,
                        Controller = controller
                    }
                        );
                }
                else if (dbEntry.State == EntityState.Deleted)
                {
                    // string keyName = dbEntry.Entity.GetType().GetProperties().Single(p => p.GetCustomAttributes(typeof(KeyAttribute), false).Count() > 0).Name;

                    // Same with deletes, do the whole record, and use either the description from Describe() or ToString()
                    result.Add(new AuditLogger()
                    {

                        EventType = "Deleted", // Deleted
                        TableName = tableName,
                        RecordID = int.Parse(dbEntry.OriginalValues.GetValue<object>("ID").ToString()),
                        // ColumnName = propertyName,
                        OriginalValue = GlobalHelper.EntityToString(dbEntry),
                        UserID = userId,
                        Time = changeTime,
                        Action = action,
                        Controller = controller
                    }
                        );
                }
                else if (dbEntry.State == EntityState.Modified)
                {
                    // var temp = dbEntry.Entity.GetType().GetProperties();

                    //  string keyName = dbEntry.Entity.GetType().GetProperties().Single(p => p.GetCustomAttributes(typeof(KeyAttribute), false).Count() > 0).Name;

                    foreach (string propertyName in dbEntry.OriginalValues.PropertyNames)
                    {
                        try
                        {

                            string old = "";
                            string newv = "";
                            try
                            {
                               if(dbEntry.OriginalValues.GetValue<object>(propertyName) != null)
                                     old = dbEntry.OriginalValues.GetValue<object>(propertyName).ToString();

                               if(dbEntry.CurrentValues.GetValue<object>(propertyName) != null)
                                    newv = dbEntry.CurrentValues.GetValue<object>(propertyName).ToString();
                            }
                            catch
                            {
                            }

                            // For updates, we only want to capture the columns that actually changed
                            if (!object.Equals(dbEntry.OriginalValues.GetValue<object>(propertyName), dbEntry.CurrentValues.GetValue<object>(propertyName)))
                            {
                                result.Add(new AuditLogger()
                                {
                                    EventType = "Modfied",    // Modified
                                    TableName = tableName,
                                    RecordID = int.Parse(dbEntry.OriginalValues.GetValue<object>("ID").ToString()),
                                    ColumnName = propertyName,
                                    OriginalValue = dbEntry.OriginalValues.GetValue<object>(propertyName) == null ? null : dbEntry.OriginalValues.GetValue<object>(propertyName).ToString(),
                                    NewValue = dbEntry.CurrentValues.GetValue<object>(propertyName) == null ? null : dbEntry.CurrentValues.GetValue<object>(propertyName).ToString(),
                                    UserID = userId,
                                    Time = changeTime,
                                    Action = action,
                                    Controller = controller
                                }
                                    );
                            }
                        }
                        catch
                        {

                        }
                    }
                }
                // Otherwise, don't do anything, we don't care about Unchanged or Detached entities
            //}
            return result;
        }

    }
}
