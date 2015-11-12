using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mapper.Contours
{
    class Chains
    {
        /*All this stuff is completely unmodified, because it works, and I didn't really want
        to mess with it*/

        public static List<List<Vector2>> Process(Dictionary<Vector2, List<Vector2>> list)
        {
            var finalChains = new List<List<Vector2>>();
            var chainStarts = new Dictionary<Vector2, List<Vector2>>();
            var chainEnds = new Dictionary<Vector2, List<Vector2>>();

            foreach (var item in list)
            {
                foreach (var end in item.Value)
                {
                    var start = item.Key;

                    var foundStart = chainStarts.ContainsKey(start);
                    var foundStart2 = chainEnds.ContainsKey(start);

                    var foundEnd = chainStarts.ContainsKey(end);
                    var foundEnd2 = chainEnds.ContainsKey(end);

                    if (!foundStart && !foundEnd && !foundStart2 && !foundEnd2)
                    {
                        var newChain = new List<Vector2>();
                        chainStarts.Add(start, newChain);
                        chainEnds.Add(end, newChain);
                        newChain.Add(start);
                        newChain.Add(end);
                    }
                    else
                    {
                        List<Vector2> cs = null;
                        List<Vector2> ce = null;
                        if (foundStart)
                        {
                            cs = chainStarts[start];
                            chainStarts.Remove(start);
                        }
                        else if (foundStart2)
                        {
                            cs = chainEnds[start];
                            chainEnds.Remove(start);
                        }

                        if (foundEnd)
                        {
                            ce = chainStarts[end];
                            chainStarts.Remove(end);
                        }
                        else if (foundEnd2)
                        {
                            ce = chainEnds[end];
                            chainEnds.Remove(end);
                        }

                        if (cs != null && ce != null)
                        {
                            if (cs.Equals(ce))
                            {
                                finalChains.Add(cs);
                                continue;
                            }
                            
                            if (foundEnd)
                            {
                                chainEnds.Remove(ce[ce.Count() - 1]);
                                if (foundStart)
                                {
                                    chainStarts.Add(ce[ce.Count() - 1], cs);
                                    JoinChains(cs, ce, true, cs, false);
                                }
                                else
                                {
                                    chainEnds.Add(ce[ce.Count() - 1], cs);
                                    JoinChains(cs, cs, false, ce, false);
                                }
                            }
                            else
                            {
                                chainStarts.Remove(ce[0]);
                                if (foundStart)
                                {
                                    chainStarts.Add(ce[0], cs);
                                    JoinChains(cs, ce, false, cs, false);
                                }
                                else
                                {
                                    chainEnds.Add(ce[0], cs);
                                    JoinChains(cs, cs, false, ce, true);
                                }
                            }
                        }
                        else
                        {
                            if (foundStart)
                            {
                                cs.Insert(0, end);
                                chainStarts.Add(end, cs);
                            }
                            else if (foundStart2)
                            {
                                cs.Add(end);
                                chainEnds.Add(end, cs);
                            }
                            if (foundEnd)
                            {
                                ce.Insert(0, start);
                                chainStarts.Add(start, ce);
                            }
                            else if (foundEnd2)
                            {
                                ce.Add(start);
                                chainEnds.Add(start, ce);
                            }
                        }
                    }
                }
            }
            return finalChains;
        }

        private static void JoinChains(List<Vector2> final, List<Vector2> start, bool flipStart, List<Vector2> end, bool flipEnd)
        {
            var result = new List<Vector2>();
            for (var i = 0; i < start.Count; i += 1)
            {
                if (flipStart)
                {
                    result.Add(start[start.Count - 1 - i]);
                }
                else
                {
                    result.Add(start[i]);
                }
            }

            for (var i = 0; i < end.Count; i += 1)
            {
                if (flipEnd)
                {
                    result.Add(end[end.Count - 1 - i]);
                }
                else
                {
                    result.Add(end[i]);
                }
            }

            final.Clear();
            foreach (var res in result)
            {
                final.Add(res);
            }
        }

        public static List<List<Vector2>> Simplify(List<List<Vector2>> chains)
        {
            var fc = new Curves.FitCurves();
            var result = new List<List<Vector2>>();
            foreach (var chain in chains)
            {
                var temp = Curves.Douglas.DouglasPeuckerReduction(chain, 8.0);
                if (temp != null && temp.Count > 1)
                {
                    result.Add(temp);
                }
            }
            return result;
        }
    }
}
