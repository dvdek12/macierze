using System;

public class Mother
{
	public string[][] mother;
	public int size;
	public Mother(string[][] mother)
	{
		this.mother = mother;
		this.size = mother.Length;
	}

	//funkcja liczaca sume glownej przekatnej macierza
	public int getSumOfMainDiagonal()
    {
		int sum = 0;

		sum += Convert.ToInt32(this.mother[0][0]);
		for (int i = 1; i < this.mother.Length; i++)
			sum += Convert.ToInt32(this.mother[i][i]);
        

		return sum;
    }

	//funkcja ktora liczy sume wierszy 
	public int getSumOfRow(int row)
    {
		if(row <= size && row > 0)
        {
			int sum = 0;

			for (int i = 0; i < this.mother[row - 1].Length; i++)
				sum += Convert.ToInt32(this.mother[row - 1][i]);

			return sum;
        }
        else
        {
			return 0;
        }
        
    }

	//funkcja ktora liczy sume kolumn 
	public int getSumOfColumn(int column)
    {
        if (column <= size && column > 0)
        {
			int sum = 0;

			for (int i = 0; i < this.mother.Length; i++)
				sum += Convert.ToInt32(this.mother[i][column - 1]);

			return sum;
        }
        else
        {
			return 0;
        }
	}

	//funkcja ktora zapisuje macierz do pliku
	public void saveToFile(string path)
    {
		//todo
    }
}
