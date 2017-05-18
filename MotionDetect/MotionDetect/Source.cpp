#include<opencv\cv.h>
#include<opencv\highgui.h>
#include <cuda.h>

using namespace cv;
using namespace std;

__global__ void getC(int *a, int *b, int *c) {
	c[threadIdx.x] = a[threadIdx.x] - b[threadIdx.x];
}

int main() {
	Mat image;
	Mat prevImage;
	vector<uchar> arrayC;
	vector<uchar> prevArrayC;
	int threshold = 20;

	VideoCapture cap;
	cap.open(0);

	namedWindow("Camera", 1);

	while (1) {
		prevImage = image;
		cap >> image;

		prevArrayC = arrayC;

		vector<uchar> arrayA;
		if (image.isContinuous()) {
			arrayA.assign(image.datastart, image.dataend);
		} else {
			for (int i = 0; i < image.rows; ++i) {
				arrayA.insert(arrayA.end(), image.ptr<uchar>(i), image.ptr<uchar>(i) + image.cols);
			}
		}

		vector<uchar> arrayB;
		if (prevImage.isContinuous()) {
			arrayB.assign(prevImage.datastart, prevImage.dataend);
		} else {
			for (int i = 0; i < prevImage.rows; ++i) {
				arrayB.insert(arrayB.end(), prevImage.ptr<uchar>(i), prevImage.ptr<uchar>(i) + prevImage.cols);
			}
		}

		int N = arrayA.size();
		int (*pA)[N], (*pB)[N], (*pC)[N];
		cudaMalloc((void**)&pA, (N)*sizeof(int));
		cudaMalloc((void**)&pB, (N)*sizeof(int));
		cudaMalloc((void**)&pC, (N)*sizeof(int));

		cudaMemcpy(pA, arrayA, (N)*sizeof(int), cudaMemcpyHostToDevice);
		cudaMemcpy(pB, arrayB, (N)*sizeof(int), cudaMemcpyHostToDevice);
		cudaMemcpy(pC, arrayC, (N)*sizeof(int), cudaMemcpyHostToDevice);

		getC <<<1, N>>>(pA, pB, pC);

		cudaMemcpy(arrayC, pC, (N*N)*sizeof(int), cudaMemcpyDeviceToHost);

		if (prevArrayC != arrayC[i]) {
			printf("Motion detected!");
		}

		cudaFree(pA);
		cudaFree(pB);
		cudaFree(pC);
	}
}