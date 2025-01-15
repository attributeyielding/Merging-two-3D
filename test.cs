using System;
using System.Collections.Generic;
using System.IO;

class MergeObjFiles
{
    static void Main(string[] args)
    {
        // Input file paths
        string objFile1 = "object1.obj"; // Replace with your first .obj file path
        string objFile2 = "object2.obj"; // Replace with your second .obj file path

        // Output file path
        string mergedFile = "merged_object.obj";

        // Merge the .obj files
        try
        {
            MergeObjFilesIntoOne(objFile1, objFile2, mergedFile);
            Console.WriteLine($"Files merged successfully into: {mergedFile}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static void MergeObjFilesIntoOne(string file1, string file2, string outputFile)
    {
        List<string> vertices = new List<string>();
        List<string> faces = new List<string>();
        int vertexOffset = 0;

        // Read the first file
        ReadObjFile(file1, vertices, faces, ref vertexOffset);

        // Read the second file and adjust indices
        ReadObjFile(file2, vertices, faces, ref vertexOffset);

        // Write to the output file
        using (StreamWriter writer = new StreamWriter(outputFile))
        {
            foreach (var vertex in vertices)
            {
                writer.WriteLine(vertex);
            }

            foreach (var face in faces)
            {
                writer.WriteLine(face);
            }
        }
    }

    static void ReadObjFile(string filePath, List<string> vertices, List<string> faces, ref int vertexOffset)
    {
        foreach (var line in File.ReadLines(filePath))
        {
            if (line.StartsWith("v ")) // Vertex line
            {
                vertices.Add(line);
            }
            else if (line.StartsWith("f ")) // Face line
            {
                string[] parts = line.Split(' ');
                string newFace = "f";

                for (int i = 1; i < parts.Length; i++) // Adjust face indices
                {
                    string[] indices = parts[i].Split('/');
                    int vertexIndex = int.Parse(indices[0]) + vertexOffset;
                    newFace += " " + vertexIndex;

                    if (indices.Length > 1)
                    {
                        if (!string.IsNullOrEmpty(indices[1])) // Texture coordinate
                        {
                            newFace += "/" + indices[1];
                        }

                        if (indices.Length > 2 && !string.IsNullOrEmpty(indices[2])) // Normal
                        {
                            newFace += "/" + indices[2];
                        }
                    }
                }

                faces.Add(newFace);
            }
        }

        // Update vertex offset
        vertexOffset += vertices.Count;
    }
}
