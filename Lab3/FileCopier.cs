using System;
using System.Text;
using System.IO;

public class FileCopier
{
	public bool FilePath (string filePath)
	{
		return File.Exists(filePath);
    }

	public bool DirectoryPath (string directoryPath)
	{
        return Directory.Exists(directoryPath);
	}

	public bool Proverka (string directoryPath, string fileName)
	{
		string newPath = Path.Combine(directoryPath, fileName);
		return File.Exists(newPath);
    }

	public void CopyFile (string source, string destDirectory, string newName, bool overwrite)
	{
		string copyPath = Path.Combine(destDirectory, newName);
		File.Copy(source, copyPath, overwrite);
	}
}
