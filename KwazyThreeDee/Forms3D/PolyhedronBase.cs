using System;
using System.Collections.Generic;

namespace Forms3D
{
    public abstract class PolyhedronBase : SharedLineMesh
    {
        protected abstract Point3D[,] Faces
        {
            get;
        }

        protected override IList<SharedLine> Generate()
        {
            List<SharedLine> sharedLines = new List<SharedLine>();
            Point3D[,] faces = Faces;

            Action<Point3D, Point3D, Vector3D> InsertIntoCollection = (pt1, pt2, normal) =>
                {
                        bool foundMatch = false;

                        for (int i = 0; i < sharedLines.Count; i++)
                        {
                            if ((sharedLines[i].Point1 == pt1 && sharedLines[i].Point2 == pt2) ||
                                (sharedLines[i].Point1 == pt2 && sharedLines[i].Point2 == pt1))
                            {
                                SharedLine sharedLine = sharedLines[i];
                                sharedLine.Normal2 = normal;
                                sharedLines[i] = sharedLine;
                                foundMatch = true;
                                break;
                            }
                        }

                        if (!foundMatch)
                        {
                            SharedLine sharedLine = new SharedLine(pt1, pt2, normal, new Vector3D());
                            sharedLines.Add(sharedLine);
                        }
                };


            for (int face = 0; face < faces.GetLength(0); face++)
            {
                Vector3D normal = Vector3D.CrossProduct(faces[face, 2] - faces[face, 0],
                                                        faces[face, 1] - faces[face, 0]);

                // For faces that are triangles.
                if (faces.GetLength(1) == 3)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Point3D point1 = faces[face, i];
                        Point3D point2 = faces[face, (i + 1) % 3];
                        InsertIntoCollection(point1, point2, normal);
                    }
                }

                // For faces that are not triangles.
                else
                {
                    int num = faces.GetLength(1) - 1;

                    for (int i = 0; i < num; i++)
                    {
                        Point3D point1 = faces[face, i + 1];
                        Point3D point2 = faces[face, (i + 1) % num + 1];
                        InsertIntoCollection(point1, point2, normal);
                    }
                }
            }

            sharedLines.TrimExcess();
            return sharedLines;
        }
    }
}

