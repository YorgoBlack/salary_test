using salary.data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace salary.data
{
    class EmployeeNode 
    {
        public Employee Node;
        public List<EmployeeNode> Children; 
    }

    public class CompanyStorage
    {
        readonly EmployeeNode root;
        int lastId;
        public CompanyStorage()
        {
            lastId = 0;
            root = new EmployeeNode()
            {
                Node = null,
                Children = new List<EmployeeNode>() 
            };
        }

        public IEnumerable<Employee> GetChildren(Employee employee)
        {
            if (employee == null) return null;
            var node = FindById(employee.Id);
            return node == null ? null : node.Children.Select(x => x.Node);
        }

        public IEnumerable<Employee> GetTop()
        {
            return root.Children.Select(x=>x.Node);
        }

        public Employee GetEmployeeById(int Id)
        {
            var rez = FindById(Id);
            return rez == null ? null : rez.Node;
        }

        EmployeeNode FindById(EmployeeNode parent, int Id)
        {
            var rez = parent.Children.FirstOrDefault(x => x.Node.Id == Id);
            if ( rez != null)
            {
                return rez;
            }
            foreach(var node in parent.Children)
            {
                rez = FindById(node, Id);
                if (rez != null) break;
            }
            return rez;
        }

        EmployeeNode FindById(int Id)
        {
            return FindById(root, Id);
        }

        public Employee Add(Employee employee)
        {
            lock(root)
            {
                employee.Id = ++lastId;
                root.Children.Add( new EmployeeNode() {
                    Node = employee, 
                    Children = new List<EmployeeNode>() 
                });
            }
            return employee;
        }

        public Employee AddChild(Employee parent, Employee employee)
        {
            if (parent == null) return null;
            lock (root)
            {
                var node = FindById(parent.Id);
                if (node != null)
                {
                    employee.Id = ++lastId;
                    node.Children.Add(new EmployeeNode()
                    {
                        Node = employee,
                        Children = new List<EmployeeNode>()
                    });
                }
                else
                {
                    employee = null;
                }
            }
            return employee;
        }

        EmployeeNode GetEmpolyeeParentById(EmployeeNode parent, EmployeeNode child)
        {
            var rez = parent.Children.FirstOrDefault(x => x.Node.Id == child.Node.Id);
            if ( rez != null )
            {
                return parent;
            }
            foreach(var node in parent.Children)
            {
                rez = GetEmpolyeeParentById(node, child);
                if (rez != null) break;
            }
            return rez;
        }

        void DeleteChildren(EmployeeNode parent, EmployeeNode child)
        {
            if( child.Children.Any() )
            {
                foreach (var node in child.Children)
                {
                    DeleteChildren(child,node);
                }
            }
            else
            {
                parent.Children.Remove(child);
            }
        }

        public void Delete(Employee employee)
        {
            if (employee == null) return;
            lock(root)
            {
                var node = FindById(employee.Id);
                if( node != null )
                {
                    var parent = GetEmpolyeeParentById(root, node);
                    if( parent != null )
                    {
                        DeleteChildren(parent, node);
                    }
                }
            }
        }


    }
}
