using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace _i
{
    class TaskListComparer : IComparer
    {
        public TaskListComparer()
        {
        }

        public int Compare(object a, object b)
        {
            try
            {
                // Convert the two passed values to ListViewItems
                var item1 = a as ListViewItem;
                var item2 = b as ListViewItem;

                // Get the unique ID's of the list items (stored in the Tag property)
                var item1Id = ((Skill)item1.Tag).PacketId;
                var item2Id = ((Skill)item2.Tag).PacketId;

                // First sort on the Checked property (unchecked items should be at the top)
                if (item1.Checked && !item2.Checked)
                    return -1;
                else if (!item1.Checked && item2.Checked)
                    return 1;

                // If both items were checked or both items were unchecked, 
                // sort by the ID (in descending order)
                if (item1Id > item2Id)
                    return 1;
                else if (item1Id < item2Id)
                    return -1;
                else
                    return 0;
            }
            catch
            {
                return 0;
            }
        }
    }
}
