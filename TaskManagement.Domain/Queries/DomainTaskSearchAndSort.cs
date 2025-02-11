using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Domain.Queries
{
    public class DomainTaskSearchAndSort
    {
        //public class DomainTaskSearchAndSort
        //{
        //    private readonly InMemoryDataBase _inMemoryDb;

        //    public DomainTaskSearchAndSort(InMemoryDataBase inMemoryDb)
        //    {
        //        _inMemoryDb = inMemoryDb;

        //    }

        //    public DomainTask SearchById(int id)
        //    {
        //      return  _inMemoryDb.Tasks.Find(t => t.Id == id) ?? 
        //              throw new OwnValidationException($"task with id : {id} was not found !");
        //    }

        //    public List<DomainTask> SearchByTitle(string name)
        //    {
        //        return _inMemoryDb.Tasks.FindAll(x => x.Title == name) ??
        //               throw new OwnValidationException($"task with title : {name} was not found! ");
        //    }

        //    public List<DomainTask> SearchByStatus(Status status)
        //    {
        //        return _inMemoryDb.Tasks.FindAll(y => y.Status == status) ??
        //               throw new OwnValidationException($" task with status : {status} was not found! ");
        //    }

        //    public List<DomainTask> SearchByAssignedUserId(int userId)
        //    {
        //        return _inMemoryDb.Tasks.FindAll(task => task.AssignedUsers.Any(user => user.Id == userId)) ??
        //               throw new OwnValidationException($" task with user id : {userId} was not found!");
        //    }

        //    public List<DomainTask> SearchByEndDate(DateTime endDate)
        //    {
        //        return _inMemoryDb.Tasks.FindAll(h => h.Status == Status.Done)
        //                   .FindAll(d => d.DeadLine == endDate) ??
        //               throw new OwnValidationException($" task with end date : {endDate} was not found!");
        //    }

        //    public List<DomainTask> SearchByDeadlineDate(DateTime deadline)
        //    {
        //        return _inMemoryDb.Tasks.FindAll(n => n.DeadLine == deadline) ??
        //               throw new OwnValidationException($" task with deadline : {deadline} was not found!");
        //    }

        //    public List<DomainTask> SortByPriority()
        //    {
        //        return _inMemoryDb.Tasks
        //                   .OrderBy(t => t.Priority)
        //                   .ToList() ??
        //               throw new OwnValidationException($" task sorted by Priority from low to high ");
        //    }

        //    public List<DomainTask> SortByDeadLine()
        //    {
        //        return _inMemoryDb.Tasks
        //                   .OrderBy(t => t.DeadLine)
        //                   .ToList() ??
        //               throw new OwnValidationException($" task sorted by dead line ");
        //    }
        //}
    }
}
