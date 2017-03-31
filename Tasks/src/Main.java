
import java.util.Random;

public class Main {
	public static void main(String arg[]) throws Exception {
		
		final int num = 4;
		int[][] dataA = new int[num][num];
		int[][] dataB = new int[num][num];

		Random rand = new Random();

		for (int i = 0; i < num; i++) {
			for (int j = 0; j < num; j++) {
				dataA[i][j] = rand.nextInt(10);
				dataB[i][j] = rand.nextInt(10);
			}
		}

		Array.printArray(dataA);
		System.out.println();
		Array.printArray(dataB);
		System.out.println();

		Array arrayA = new Array(dataA);
		Array arrayB = new Array(dataB);
		System.out.println(arrayA.multiply(arrayB));

	}

}