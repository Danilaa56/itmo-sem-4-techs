import ru.danilaa56.benchmarking.MeasureUtils;

public class InteropMain {

    public static void main(String[] args) {
        int n = 100_000_000;
        int[] numbers = new int[n];

        for(int i = 0; i < n; numbers[i] = i++) {
            numbers[i] = i;
        }

        System.out.println(MeasureUtils.measure(() -> sum(numbers), 5, "JNI sum"));
        System.out.println(MeasureUtils.measure(() -> jSum(numbers), 5, "Java sum"));
    }

    public static native int sum(int[] array);

    public static int jSum(int[] array) {
        int sum = 0;

        for (int j : array) {
            sum += j;
        }

        return sum;
    }

    static {
        System.loadLibrary("libNative");
    }
}
