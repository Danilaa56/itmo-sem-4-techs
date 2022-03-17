cmake_minimum_required(VERSION 3.21)
project(Native)

set(CMAKE_CXX_STANDARD 23)

add_library(Native SHARED library.cpp)
