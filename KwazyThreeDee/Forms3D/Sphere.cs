using System;
using System.Collections.Generic;

namespace Forms3D
{
    class Sphere : SharedLineMesh
    {
        int slices = 18, stacks = 9;
        double radius = 2;
        Point3D center = new Point3D(0, 0, 0);

        protected override IList<SharedLine> Generate()
        {
            List<Point3D> vertices = new List<Point3D>();
            List<Vector3D> normals = new List<Vector3D>();

            // From top to bottom.
            for (int stack = 0; stack <= stacks; stack++)
            {
                double phi = Math.PI / 2 - stack * Math.PI / stacks;
                double y = radius * Math.Sin(phi);
                double scale = -radius * Math.Cos(phi);

                // Around the world.
                for (int slice = 0; slice <= slices; slice++)
                {
                    double theta = slice * 2 * Math.PI / slices;
                    double x = scale * Math.Sin(theta);
                    double z = scale * Math.Cos(theta);

                    Vector3D normal = new Vector3D(x, y, z);
                    normals.Add(normal);
                    vertices.Add(normal + center);
                }
            }

            List<SharedLine> sharedLines = new List<SharedLine>();

            // Get all the shared lines.
            for (int stack = 0; stack < stacks; stack++)
            {
                for (int slice = 0; slice < slices; slice++)
                {
                    // Lines of latitude.
                    int index1 = (stack + 0) * (slices + 1) + slice;
                    int index2 = (stack + 1) * (slices + 1) + slice;

                    sharedLines.Add(new SharedLine(vertices[index1], vertices[index2], normals[index1], normals[index2]));

                    if (stack != 0)
                    {
                        index2 = index1 + 1;
                    }

                    sharedLines.Add(new SharedLine(vertices[index1], vertices[index2], normals[index1], normals[index2]));
                }
            }

            sharedLines.TrimExcess();
            return sharedLines;
        }
    }
}
