package ru.danilaa56.benchmarking;

import java.util.LinkedList;

public class MeasureUtils {
    public static <T> T measure(MeasureUtils.MeasureTask<T> runnable, int count, String label) {
        try {
            LinkedList<Long> times = new LinkedList<>();
            T result = null;

            for(int i = 0; i < count; ++i) {
                long time = System.nanoTime();
                result = runnable.run();
                time = System.nanoTime() - time;
                System.out.printf("[DEBUG] %s: took %d ms\n", label, time / 1000000L);
                times.add(time);
            }

            times.sort(Long::compareTo);
            if (count > 1) {
                times.removeLast();
            }

            if (count > 1) {
                times.removeLast();
            }

            long timeSum = times.stream().reduce(0L, Long::sum);
            System.out.printf("[DEBUG] %s: took %d ms in average\n", label, timeSum / (long)times.size() / 1000000L);
            return result;
        } catch (Exception var8) {
            throw new RuntimeException(var8);
        }
    }

    public interface MeasureTask<T> {
        T run() throws Exception;
    }
}
