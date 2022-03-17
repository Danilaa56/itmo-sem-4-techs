import java.io.BufferedReader;
import java.io.FileReader;
import java.util.Arrays;
import java.util.HashMap;

public class TryScalaMain {

    /**
     * Read file args[0] and count number of each word
     *
     * @param args
     * @throws Exception
     */
    public static void main(String[] args) throws Exception {
        (new ScalaMain()).main(args);
        System.out.println("================================================================================");
        (new BufferedReader(new FileReader(args[0]))).lines()
                .flatMap(line -> Arrays.stream(line.split(" ")))
                .map(String::trim)
                .filter(word -> word.matches("^[а-яё]+$"))
                .collect(HashMap::new, (map, str) -> map.put(str, (Integer) map.getOrDefault(str, 0) + 1),
                        (a, b) -> {})
                .forEach((key, value) -> System.out.println(key + " " + value));
    }
}
