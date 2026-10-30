using System;
using System.Collections.Generic;
using System.Linq;
using ThreeJs4Net.Core;
using ThreeJs4Net.Math;
using Attribute = ThreeJs4Net.Renderers.Shaders.Attribute;

namespace ThreeJs4Net.Examples.Jsm.Utils
{
    public static class BufferGeometryUtils
    {
        public static void ToTrianglesDrawMode(BufferGeometry geometry, int drawMode)
        {
            throw new NotImplementedException();
        }
        
        public static void MergeVertices(BufferGeometry geometry, int tolerance)
        {
            throw new NotImplementedException();
        }

        public static void EstimateBytesUsed(BufferGeometry[] geometry)
        {
            throw new NotImplementedException();
        }

        public static void InterleaveAttributes(IBufferAttribute[] attributes)
        {
            throw new NotImplementedException();
        }

        public static void ComputeTangents(BufferGeometry geometry)
        {
            var index = geometry.Index;
            var attributes = geometry.Attributes;

            // based on http://www.terathon.com/code/tangent.html
            // (per vertex tangents)

            if (index == null ||
                 attributes["position"] == null ||
                 attributes["normal"] == null ||
                 attributes["uv"] == null)
            {
                throw new Exception("BufferGeometryUtils: .computeTangents() failed. Missing required attributes (index, position, normal or uv)");
            }

            var indices = index.Array;
            var positions = geometry.GetAttribute<float>("position").Array;
            var normals = geometry.GetAttribute<float>("normal").Array;
            var uvs = geometry.GetAttribute<float>("uv").Array;

            var nVertices = positions.Length / 3;

            if (attributes["tangent"] == null)
            {
                geometry.SetAttribute("tangent", new BufferAttribute<float>(new float[4 * nVertices], 4));
            }

            var tangents = geometry.GetAttribute<float>("tangent").Array;

            var tan1 = new List<Vector3>();
            var tan2 = new List<Vector3>();

            for (var i = 0; i < nVertices; i++)
            {
                tan1[i] = new Vector3();
                tan2[i] = new Vector3();
            }

            var vA = new Vector3();
            var vB = new Vector3();
            var vC = new Vector3();
            var uvA = new Vector2();
            var uvB = new Vector2();
            var uvC = new Vector2();
            var sdir = new Vector3();
            var tdir = new Vector3();

            void handleTriangle(int a, int b, int c)
            {
                vA.FromArray(positions, a * 3);
                vB.FromArray(positions, b * 3);
                vC.FromArray(positions, c * 3);

                uvA.FromArray(uvs, a * 2);
                uvB.FromArray(uvs, b * 2);
                uvC.FromArray(uvs, c * 2);

                vB.Sub(vA);
                vC.Sub(vA);

                uvB.Sub(uvA);
                uvC.Sub(uvA);

                float r = (float)1.0 / (uvB.X * uvC.Y - uvC.X * uvB.Y);

                // silently ignore degenerate uv triangles having coincident or colinear vertices
                if (float.IsInfinity(r))
                {
                    return;
                }

                sdir.Copy(vB).MultiplyScalar(uvC.Y).AddScaledVector(vC, -uvB.Y).MultiplyScalar(r);
                tdir.Copy(vC).MultiplyScalar(uvB.X).AddScaledVector(vB, -uvC.X).MultiplyScalar(r);

                tan1[a].Add(sdir);
                tan1[b].Add(sdir);
                tan1[c].Add(sdir);

                tan2[a].Add(tdir);
                tan2[b].Add(tdir);
                tan2[c].Add(tdir);
            }

            var groups = geometry.groups;

            if (groups.Count == 0)
            {
                groups = new List<BufferGeometryGroups>
                {
                    new BufferGeometryGroups() {Start = 0, Count = indices.Length}
                };
            }

            for (var i = 0; i < groups.Count; ++i)
            {
                var group = groups[i];
                var start = group.Start;
                var count = group.Count;

                for (var j = start; j < start + count; j += 3)
                {
                    handleTriangle(
                        (int)indices[j + 0],
                        (int)indices[j + 1],
                        (int)indices[j + 2]
                    );
                }
            }

            var tmp = new Vector3();
            var tmp2 = new Vector3();
            var n = new Vector3();
            var n2 = new Vector3();
            Vector3 t;
            float test;
            float w;

            void handleVertex(int v)
            {
                n.FromArray(normals, v * 3);
                n2.Copy(n);

                t = tan1[v];

                // Gram-Schmidt orthogonalize
                tmp.Copy(t);
                tmp.Sub(n.MultiplyScalar(n.Dot(t))).Normalize();

                // Calculate handedness
                tmp2.CrossVectors(n2, t);
                test = tmp2.Dot(tan2[v]);
                w = (test < 0.0) ? (float)-1.0 : (float)1.0;

                tangents[v * 4] = tmp.X;
                tangents[v * 4 + 1] = tmp.Y;
                tangents[v * 4 + 2] = tmp.Z;
                tangents[v * 4 + 3] = w;
            }

            for (var i = 0; i < groups.Count; ++i)
            {
                var group = groups[i];
                var start = group.Start;
                var count = group.Count;

                for (var j = start; j < start + count; j += 3)
                {
                    handleVertex((int)indices[j + 0]);
                    handleVertex((int)indices[j + 1]);
                    handleVertex((int)indices[j + 2]);
                }
            }
        }

        /**
        * @param  {Array<BufferGeometry>} geometries
        * @param  {Boolean} useGroups
        * @return {BufferGeometry}
        */
        public static BufferGeometry MergeBufferGeometries(BufferGeometry[] geometries, bool useGroups = false)
        {
            var isIndexed = geometries[0].Index != null;

            var attributesUsed = geometries[0].AttributesKeys; //new List<string>(); // new Set(Object.keys(geometries[0].attributes));

            //TODO:
            //var morphAttributesUsed = new Set(Object.keys(geometries[0].morphAttributes));
            var attributes = new Dictionary<string, List<IBufferAttribute>>();

            //TODO:
            //var morphAttributes = { };

            //TODO:
            //var morphTargetsRelative = geometries[0].morphTargetsRelative;

            var mergedGeometry = new BufferGeometry();

            var offset = 0;

            for (var i = 0; i < geometries.Count(); ++i)
            {
                var geometry = geometries[i];
                var attributesCount = 0;

                // ensure that all geometries are indexed, or none
                if (isIndexed != (geometry.Index != null))
                {
                    throw new Exception("BufferGeometryUtils: .mergeBufferGeometries() failed with geometry at index ' + i + '. All geometries must have compatible attributes; make sure index attribute exists among all geometries, or in none of them.");
                }

                // gather attributes, exit early if they're different
                foreach (var name in geometry.AttributesKeys)
                {
                    if (!attributesUsed.Contains(name))
                    {
                        throw new Exception($"BufferGeometryUtils: .mergeBufferGeometries() failed with geometry at index {i}. All geometries must have compatible attributes; make sure '{name}' attribute exists among all geometries, or in none of them.");
                    }

                    //TODO:
                    if (!attributes.TryGetValue(name, out var list))
                    {
                        list = new List<IBufferAttribute>(geometries.Length);
                        attributes[name] = list;
                    }
                    list.Add(geometry.Attributes[name] as IBufferAttribute);

                    attributesCount++;
                }

                // ensure geometries have the same number of attributes

                if (attributesCount != attributesUsed.Count)
                {
                    throw new Exception($"BufferGeometryUtils: .mergeBufferGeometries() failed with geometry at index {i}. Make sure all geometries have the same number of attributes.");
                }

                // gather morph attributes, exit early if they're different

                //TODO:
                //if (morphTargetsRelative != geometry.morphTargetsRelative)
                //{
                //    console.error('THREE.BufferGeometryUtils: .mergeBufferGeometries() failed with geometry at index ' + i + '. .morphTargetsRelative must be consistent throughout all geometries.');
                //    return null;
                //}

                //TODO:
                //for (var name in geometry.morphAttributes)
                //         {
                //             if (!morphAttributesUsed.has(name))
                //             {
                //                 console.error('THREE.BufferGeometryUtils: .mergeBufferGeometries() failed with geometry at index ' + i + '.  .morphAttributes must be consistent throughout all geometries.');
                //                 return null;
                //             }
                //             if (morphAttributes[name] === undefined) morphAttributes[name] = [];
                //             morphAttributes[name].push(geometry.morphAttributes[name]);
                //         }

                // gather .userData

                //TODO:
                //mergedGeometry.userData.mergedUserData = mergedGeometry.userData.mergedUserData || [];
                //mergedGeometry.userData.mergedUserData.push(geometry.userData);

                if (useGroups)
                {
                    int count;

                    if (isIndexed)
                    {
                        count = geometry.Index.Count;
                    }
                    else if (geometry.Attributes["position"] != null)
                    {
                        var position = geometry.GetAttribute<float>("position");
                        count = position.Count;
                    }
                    else
                    {
                        throw new Exception($"BufferGeometryUtils: .mergeBufferGeometries() failed with geometry at index {i}. The geometry must have either an index or a position attribute");
                    }

                    mergedGeometry.AddGroup(offset, count, i);
                    offset += count;
                }
            }

            // merge indices
            if (isIndexed)
            {
                var indexOffset = 0;

                // Size the merged index up front instead of growing a List<uint>. A non-positive Count
                // (e.g. ItemSize -1) contributes nothing, exactly like the original "j < index.Count" loop.
                var totalCount = 0;
                for (var i = 0; i < geometries.Length; ++i)
                {
                    var count = geometries[i].Index.Count;
                    if (count > 0)
                    {
                        totalCount += count;
                    }
                }

                var values = new uint[totalCount];
                var k = 0;

                for (var i = 0; i < geometries.Length; ++i)
                {
                    var geometry = geometries[i];
                    var position = geometry.GetAttribute<float>("position");
                    var index = geometry.Index;

                    // Same as index.GetX(j) for each j, reading Array/ItemSize once (dictionary lookups).
                    var indexArray = index.Array;
                    var itemSize = index.ItemSize;
                    var count = indexArray.Length / itemSize;

                    for (var j = 0; j < count; ++j)
                    {
                        values[k++] = (uint)(indexArray[j * itemSize] + indexOffset);
                    }

                    indexOffset += position.Count;
                }

                // As in three.js (new BufferAttribute(mergedIndex, 1)). It used to be created with the
                // parameterless constructor, leaving ItemSize = -1 and Type = null, so Count was negative.
                var mergedIndex = new BufferAttribute<uint>(values, 1);
                mergedGeometry.SetIndex(mergedIndex);
            }

            // merge attributes
            foreach (var attr in attributes)
            {
                var name = attr.Key;

                IBufferAttribute mergedAttribute = null;
                var toMerge = attr.Value.ToArray();

                switch (toMerge[0].Type.Name)
                {
                    case "String":
                        mergedAttribute = MergeBufferAttributes<string>(toMerge);
                        break;
                    case "Byte":
                        mergedAttribute = MergeBufferAttributes<byte>(toMerge);
                        break;
                    case "Int16":
                        mergedAttribute = MergeBufferAttributes<Int16>(toMerge);
                        break;
                    case "Int32":
                        mergedAttribute = MergeBufferAttributes<int>(toMerge);
                        break;
                    case "UInt32":
                        mergedAttribute = MergeBufferAttributes<uint>(toMerge);
                        break;
                    case "UInt16":
                        mergedAttribute = MergeBufferAttributes<UInt16>(toMerge);
                        break;
                    case "Single":
                        mergedAttribute = MergeBufferAttributes<float>(toMerge);
                        break;
                }

                if (mergedAttribute == null)
                {
                    throw new Exception($"BufferGeometryUtils: .mergeBufferGeometries() failed while trying to merge the {name} attribute.");
                }

                mergedGeometry.SetAttribute(name, (Attribute) mergedAttribute);
            }

            // merge morph attributes
            //TODO:
            //for (var name in morphAttributes)
            //      {
            //          var numMorphTargets = morphAttributes[name][0].length;
            //          if (numMorphTargets === 0) break;
            //          mergedGeometry.morphAttributes = mergedGeometry.morphAttributes || { };
            //          mergedGeometry.morphAttributes[name] = [];
            //          for (var i = 0; i < numMorphTargets; ++i)
            //          {
            //              var morphAttributesToMerge = [];
            //              for (var j = 0; j < morphAttributes[name].length; ++j)
            //              {
            //                  morphAttributesToMerge.push(morphAttributes[name][j][i]);
            //              }
            //              var mergedMorphAttribute = this.mergeBufferAttributes(morphAttributesToMerge);
            //              if (!mergedMorphAttribute)
            //              {
            //                  console.error('THREE.BufferGeometryUtils: .mergeBufferGeometries() failed while trying to merge the ' + name + ' morphAttribute.');
            //                  return null;
            //              }
            //              mergedGeometry.morphAttributes[name].push(mergedMorphAttribute);
            //          }
            //      }
            return mergedGeometry;
        }

        public static BufferAttribute<T> MergeBufferAttributes<T>(IBufferAttribute[] attributes)
        {
            Type typedArray = null;
            int? itemSize = null;
            bool? normalized = null;
            var arrayLength = 0;

            for (var i = 0; i < attributes.Length; ++i)
            {
                var attribute = attributes[i] as BufferAttribute<T>;

                if (attribute.ContainsKey("isInterleavedBufferAttribute"))
                {
                    throw new Exception("BufferGeometryUtils: .mergeBufferAttributes() failed. InterleavedBufferAttributes are not supported.");
                }
                var attributeType = attribute.Type;
                if (typedArray == null) typedArray = attributeType;
                if (typedArray != attributeType)
                {
                    throw new Exception("BufferGeometryUtils: .mergeBufferAttributes() failed. BufferAttribute.array must be of consistent array types across matching attributes.");
                }

                var attributeItemSize = attribute.ItemSize;
                if (itemSize == null) itemSize = attributeItemSize;
                if (itemSize != attributeItemSize)
                {
                    throw new Exception("BufferGeometryUtils: .mergeBufferAttributes() failed. BufferAttribute.itemSize must be consistent across matching attributes.");
                }

                var attributeNormalized = attribute.Normalized;
                if (normalized == null) normalized = attributeNormalized;
                if (normalized != attributeNormalized)
                {
                    throw new Exception("BufferGeometryUtils: .mergeBufferAttributes() failed. BufferAttribute.normalized must be consistent across matching attributes.");
                }
                arrayLength += attribute.Array.Length;
            }

            var array = new T[arrayLength];
            var offset = 0;

            for (var i = 0; i < attributes.Length; ++i)
            {
                var source = (attributes[i] as BufferAttribute<T>).Array;

                source.CopyTo(array, offset);
                offset += source.Length;
            }
            
            return new BufferAttribute<T>(array, (int)itemSize, (bool)normalized);
        }
    }
}
