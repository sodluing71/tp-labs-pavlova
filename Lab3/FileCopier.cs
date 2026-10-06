using System;
using System.Text;
using System.IO;

public class FileCopier
{
	public bool FilePath (string filePath)
	{
		File.Exists(filePath);
    }

	public bool Directory(string directoryPath)
	{
		Directory.Exists(directoryPath);
	}

	public bool Proverka (string directoryPath, string fileName)
	{
		sring newPath = newPath.Combine(directoryPath, fileName);
		return File.Exists(newPath);
    }

	public void CopyFile (string source, string destDirectory, string newName, bool overwrite)
	{
		string copyPath = Path.Combine(destDirectory, newName);
		File.Copy(source, copyPath, overwrite);
	}
}
