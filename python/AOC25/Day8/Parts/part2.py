from pathlib import Path

from Parts.util.handlers import build_graph, handle_edge


class Part2:
    def __init__(self, input_file: Path):
        with open(input_file, "r") as f:
            self.input_data = f.readlines()

    def get_day_solution(self):
        vertices, edges = build_graph(self.input_data)

        # we keep track of distance to wall for latest edge below
        distance = 0
        last_edge = ()

        # we build the circuits, until there's only one
        circuits = {}
        counter = 1
        for edge in edges:
            if counter > 1 and len(circuits) == 1:
                boxes = list(circuits.values())[0]
                if len(boxes) == len(vertices):
                    v1, v2 = last_edge
                    distance = v1["pos"][0] * v2["pos"][0]
                    break

            v1, v2 = edge[1]
            counter = handle_edge(circuits, counter, v1, v2)

            last_edge = v1, v2

        return str(distance)
