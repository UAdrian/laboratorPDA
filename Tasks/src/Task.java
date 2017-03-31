import java.util.concurrent.Callable;

public class Task implements Callable<Integer> {
	private int rowId;
	private int colId;

	private Array arrayA;
	private Array arrayB;

	/**
	 * 
	 * @param matrixA
	 * @param matrixB
	 * @param rowId
	 * @param colId
	 */
	public Task(Array matrixA, Array matrixB, int rowId, int colId) {
		this.rowId = rowId;
		this.colId = colId;
		this.arrayA = matrixA;
		this.arrayB = matrixB;
	}

	@Override
	public Integer call() throws Exception {
		String msg = String.format("A[%d] * B[%d]", rowId, colId);
		System.out.println("Started  Task -> " + msg);
		int product = 0;
		for (int i = 0; i < arrayA.getCol(); ++i)
			product = product + arrayA.data[rowId][i] * arrayB.data[i][colId];
		System.out.println("Finished Task -> " + msg + " = " + product);
		return product;
	}

}
