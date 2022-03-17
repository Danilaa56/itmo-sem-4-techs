import scala.reflect.io.File

class ScalaMain {
  def printTuple(tuple: (String, Int)): Unit = {
    println(f"${tuple._1} -> ${tuple._2}")
  }

  def printMap(map: Map[String, Int]): Unit = {
    map.foreach(tuple => {
      printTuple(tuple)
    })
  }

  def main(args: Array[String]): Unit = {
    File(args.apply(0)).lines.toList.flatMap(line => line.split(" "))
      .map(word => word.trim.toLowerCase)
      .filter(word => word.matches("^[а-яё]+$"))
      .groupBy(word => word)
      .map(word => word._1 -> word._2.length)
      .tapEach(tuple => printTuple(tuple))
  }
}
