package ru.itmo.danilaa56;

import java.util.Arrays;
import java.util.Random;
import java.util.concurrent.TimeUnit;
import org.openjdk.jmh.Main;
import org.openjdk.jmh.annotations.Benchmark;
import org.openjdk.jmh.annotations.BenchmarkMode;
import org.openjdk.jmh.annotations.Fork;
import org.openjdk.jmh.annotations.Measurement;
import org.openjdk.jmh.annotations.Mode;
import org.openjdk.jmh.annotations.OutputTimeUnit;
import org.openjdk.jmh.annotations.Scope;
import org.openjdk.jmh.annotations.State;
import org.openjdk.jmh.annotations.Warmup;

@State(Scope.Thread)
@Fork(1)
@BenchmarkMode({Mode.AverageTime})
@OutputTimeUnit(TimeUnit.MILLISECONDS)
@Warmup(
        iterations = 1,
        time = 1000,
        timeUnit = TimeUnit.MILLISECONDS
)
@Measurement(
        iterations = 1,
        time = 1000,
        timeUnit = TimeUnit.MILLISECONDS
)
public class Bench {
    private static double[] data;

    public static void main(String[] args) throws Exception {
        Main.main(args);
    }

    @Benchmark
    public static void setup() {
        data = randomArray(10000);
    }

    public static double[] randomArray(int size) {
        double[] array = new double[size];
        Random random = new Random(56L);

        for(int i = 0; i < size; ++i) {
            array[i] = random.nextDouble();
        }

        return array;
    }

    @Benchmark
    public static void bubbleSort() {
        setup();
        int changes = 1;

        while(changes > 0) {
            changes = 0;

            for(int i = 1; i < data.length; ++i) {
                if (data[i - 1] > data[i]) {
                    double tmp = data[i];
                    data[i] = data[i - 1];
                    data[i - 1] = tmp;
                    ++changes;
                }
            }
        }

    }

    @Benchmark
    public void javaSort() {
        setup();
        Arrays.sort(data);
    }
}
