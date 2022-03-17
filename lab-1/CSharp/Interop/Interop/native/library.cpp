extern "C" {
int __declspec(dllexport) Sum(int *numbers, int length) {
    int sum = 0;

    for (int i = 0; i < length; i++) {
        sum += numbers[i];
    }

    return sum;
}
}
