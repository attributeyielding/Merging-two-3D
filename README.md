# Merging-two-3D
Merging two 3D ```.obj``` files into one can be done by parsing the vertex and face data from both files and combining them while updating the face indices of the second object to account for the vertices of the first object.

Here is a simple C# script for merging two ```.obj``` files into one:

```
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

```
# How It Works
1. Reading .obj Files:
The script reads vertex (```v```) and face (```f```) lines from each ```.obj``` file.
2. Updating Face Indices:
When merging the second file, the face indices are updated to account for the number of vertices in the first file.
3. Writing the Merged File:
Combines all vertices and faces into a single ```.obj``` file.

# Instructions
1. Replace object1.obj and object2.obj with the paths to your .obj files.
2. Save the script as MergeObjFiles.cs.
3. Compile and run the script:

```
mcs MergeObjFiles.cs -out:MergeObjFiles.exe
mono MergeObjFiles.exe
```

4. The merged ```.obj``` file will be saved as ```merged_object.obj```.
5. 

# Let me know if you need further assistance! 😊







