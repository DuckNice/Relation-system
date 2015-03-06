using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NRelationSystem
{
    public partial class RelationSystem
    {
        public void AddRolesToMask(string maskName, string[] roles = null)
        {
            if (roles != null)
            {
                foreach (string role in roles)
                {
                    if (role != "")
                    {
                        peopleAndMasks.AddRoleToMask(maskName, role);
                    }
                }
            }
        }


        public void AddAction(MAction action)
        {
            posActions.Add(action.name, action);
        }
    }
}
