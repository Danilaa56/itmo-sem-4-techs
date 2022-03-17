cmake_minimum_required(VERSION 3.21)
project(Native)

set(CMAKE_CXX_STANDARD 23)

include_directories(headers)
include_directories("C:\\Program Files\\Java\\jdk-17.0.2\\include")
include_directories("C:\\Program Files\\Java\\jdk-17.0.2\\include\\win32")

add_library(Native SHARED InteropMain.cpp)
