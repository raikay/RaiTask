using RaiTask.IServices.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RaiTask.API.Dtos
{
    public class ModifyJobInput
    {
        public ScheduleDto NewScheduleEntity { get; set; }
        public ScheduleDto OldScheduleEntity { get; set; }
    }
}
