
import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.concurrent.Future;

public class Array {
	private static final int THREAD_COUNT = 3;

	private int row;

	private int col;

	int data[][];

	/**
	 * 
	 * @param data
	 */
	public Array(int data[][]) {
		this.data = data;
		this.row = data.length;
		this.col = data[0].length;
	}
	
	/**
	 * @return the row
	 */
	public int getRow() {
		return row;
	}

	/**
	 * @return the col
	 */
	public int getCol() {
		return col;
	}

	/**
	 * 
	 * @param array
	 * @return the computed array
	 */
	public Array multiply(Array array) {
		ExecutorService executor = Executors.newFixedThreadPool(THREAD_COUNT);

		try {
			if (this.col != array.row)
				throw new IllegalArgumentException("Array 1 cols needs to be equal to array 2 rows!");

			List<Task> listInit = Array.getTasks(this, array);
			List<Future<Integer>> listResult = executor.invokeAll(listInit);

			int data[][] = new int[this.row][array.col];
			for (int i = 0; i < this.row; ++i)
				for (int j = 0; j < array.col; ++j)
					data[i][j] = listResult.get(i * array.col + j).get();

			return new Array(data);
		} catch (Exception e) {
			executor.shutdown();
		}
		return null;
	}

	/**
	 * 
	 * @param arrayA
	 * @param arrayB
	 * @return initial task list
	 */
	public static List<Task> getTasks(Array arrayA, Array arrayB) {
		List<Task> listInit = new ArrayList<>();
		for (int i = 0; i < arrayA.row; ++i)
			for (int j = 0; j < arrayB.col; ++j)
				listInit.add(new Task(arrayA, arrayB, i, j));
		return listInit;
	}
	
	public String toString() {
		StringBuilder builder = new StringBuilder();
		for (int i = 0; i < row; ++i) {
			for (int j = 0; j < col; ++j)
				builder.append(String.format(" %3d ", data[i][j]));
			builder.append('\n');
		}
		return builder.toString();
	}

	/**
	 * 
	 * @param array
	 */
	public static void printArray(int array[][]) {
		for (int i = 0; i < array.length; i++) {
			for (int j = 0; j < array.length; j++) {
				System.out.print(array[i][j] + " ");
			}
			System.out.println();
		}
	}

}