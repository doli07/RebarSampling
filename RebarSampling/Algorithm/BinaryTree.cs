using Org.BouncyCastle.Asn1.Cmp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebarSampling
{
    /// <summary>
    /// 二叉搜索树节点的数据结构
    /// </summary>
    public class BiTreeNode
    {
        /// <summary>
        /// 当前节点值
        /// </summary>
        public Rebar val { get; set; }
        /// <summary>
        /// 当前节点的左孩子
        /// </summary>
        public BiTreeNode left { get; set; }
        /// <summary>
        /// 当前节点的右孩子
        /// </summary>
        public BiTreeNode right { get; set; }

        public BiTreeNode()
        {
            this.val = new Rebar();
            this.left = null;
            this.right = null;
        }
        public BiTreeNode(Rebar x)
        {
            this.val = x;//注意：此处使用的是引用，不是深度拷贝
            this.left = null;
            this.right = null;
        }
    }

    public class Solution
    {
        /// <summary>
        /// 使用二叉树，找到长度与待组合rebar匹配的rebar节点
        /// </summary>
        /// <param name="_root">当前节点</param>
        /// <param name="_rebar">待组合的rebar</param>
        /// <param name="_material">原材组合目标</param>
        /// <param name="_threshold">阈值</param>
        /// <returns></returns>
        public static BiTreeNode FindNode( BiTreeNode _root,  Rebar _rebar, MaterialOri _material, int _threshold = 0)
        {
            if (_root == null) return null;

            if (_material._length < (_root.val.length + _rebar.length)) return null;//原材长度不足，退出


            List<Rebar> _newlist = new List<Rebar>();

            _newlist.Add(_root.val);
            _newlist.Add(_rebar);

            if (IfContain(_newlist, _material, _threshold))//两根长度能包含进原材
            {
                _root.val.TaoUsed = true;
                _rebar.TaoUsed = true;
                return _root;
            }
            else
            {
                return ((_material._length - _rebar.length) > _root.val.length) ? 
                    FindNode(_root.right,  _rebar, _material, _threshold) : 
                    FindNode(_root.left,  _rebar, _material, _threshold);//根据剩余长度不同，分别选择左孩递归还是右孩递归，核心算法
            }

        }
        public static BiTreeNode FindNode(BiTreeNode _root, List<Rebar> _list, MaterialOri _material, int _threshold = 0)
        {
            if (_root == null) return null;

            if (_material._length < (_root.val.length + _list.Sum(t=>t.length))) return null;//原材长度不足，退出


            List<Rebar> _newlist = new List<Rebar>();

            _newlist.Add(_root.val);
            _newlist.AddRange(_list);

            if (IfContain(_newlist, _material, _threshold))//两根长度能包含进原材
            {
                _root.val.TaoUsed = true;
                foreach(var item in _list)
                {
                    item.TaoUsed=true;
                }
                //_rebar.TaoUsed = true;
                return _root;
            }
            else
            {
                return ((_material._length - _list.Sum(t=>t.length)/*_rebar.length*/) > _root.val.length) ?
                    FindNode(_root.right, _list, _material, _threshold) :
                    FindNode(_root.left, _list, _material, _threshold);//根据剩余长度不同，分别选择左孩递归还是右孩递归，核心算法
            }

        }



        /// <summary>
        /// 根据list，创建平衡二叉查询树，递归
        /// </summary>
        /// <param name="_list"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static BiTreeNode CreateTree(List<Rebar> _list, int left, int right)
        {
            if (left > right) return null;
            int mid = (left + right) / 2;

            BiTreeNode root = new BiTreeNode(_list[mid]);
            root.left = CreateTree(_list, left, mid - 1);
            root.right = CreateTree(_list, mid + 1, right);
            return root;
        }

        /// <summary>
        /// 判断rebar序列的总长度是否与原材长度接近，或差值低于阈值
        /// </summary>
        /// <param name="_rebarlist">待确定的长度</param>
        /// <param name="_material">原材</param>
        /// <param name="_threshold">阈值</param>
        /// <returns></returns>
        private static bool IfContain(List<Rebar> _rebarlist, MaterialOri _material, int _threshold = 0)
        {
            if ((_material._length - _rebarlist.Sum(t => t.length)) >= 0 && (_material._length - _rebarlist.Sum(t => t.length)) <= _threshold)//所需长度与原材库的长度相差:0≤x＜500
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    ////不能用分别在左右子树两部分来处理这种思想，因为两个待求的节点可能分别在左右子树中
    //public class SolutionDouble
    //{
    //    public bool FindTarget(BiTreeNode root, int k)
    //    {
    //        HashSet<int> set = new HashSet<int>();
    //        return DFS(root, k, set);
    //    }

    //    public bool DFS(BiTreeNode root, int k, HashSet<int> set)
    //    {
    //        if (root == null)
    //        {
    //            return false;
    //        }

    //        if (set.Contains(k - root.val))
    //        {
    //            return true;
    //        }

    //        set.Add(root.val);

    //        return DFS(root.left, k, set) || DFS(root.right, k, set);
    //    }
    //}

    //public class Solution
    //{
    //    public BiTreeNode DeleteNodesWithSum(BiTreeNode root, int k)
    //    {
    //        HashSet<int> set = new HashSet<int>();
    //        return DFS(root, k, set);
    //    }

    //    public BiTreeNode DFS(BiTreeNode root, int k, HashSet<int> set)
    //    {
    //        if (root == null)
    //        {
    //            return null;
    //        }

    //        BiTreeNode leftResult = DFS(root.left, k, set);
    //        if (leftResult != null)
    //        {
    //            return leftResult;
    //        }

    //        if (set.Contains(k - root.val))
    //        {
    //            return root;
    //        }

    //        set.Add(root.val);

    //        BiTreeNode rightResult = DFS(root.right, k, set);
    //        if (rightResult != null)
    //        {
    //            return rightResult;
    //        }

    //        return null;
    //    }
    //}


    //public class SolutionTriple
    //{
    //    public bool FindTriplet(BiTreeNode root, int k)
    //    {
    //        List<int> nodeList = new List<int>();
    //        return DFS(root, k, 3, nodeList);
    //    }

    //    public bool DFS(BiTreeNode root, int target, int count, List<int> nodeList)
    //    {
    //        if (root == null)
    //        {
    //            return false;
    //        }

    //        nodeList.Add(root.val);

    //        if (count == 1)
    //        {
    //            return nodeList.Sum() == target;
    //        }

    //        if (DFS(root.left, target, count - 1, nodeList))
    //        {
    //            return true;
    //        }

    //        if (DFS(root.right, target, count - 1, nodeList))
    //        {
    //            return true;
    //        }

    //        nodeList.RemoveAt(nodeList.Count - 1);

    //        return DFS(root.left, target, count, new List<int>(nodeList)) || DFS(root.right, target, count, new List<int>(nodeList));
    //    }
    //}






}
