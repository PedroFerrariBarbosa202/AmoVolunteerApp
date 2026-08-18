using VolunteerApp.Content.Overlays;

namespace VolunteerApp.Services {
    public class WarningOverlayData {
        public string? Title { get; set; }
        public string? Message { get; set; }
    };

    internal static class OverlayManager {
        static ContentView? CurrentOverlay = null;
        public static void SetWarningOverlay(WarningOverlayData data, Layout layout) {
            if(layout is Grid root_grid) {
                WarningOverlay overlay_view = new WarningOverlay(data);
                root_grid.Children.Add(overlay_view);

                CurrentOverlay = overlay_view;
            }
        }

        public static void SetLoadingOverlay(Layout layout) {
            if (layout is Grid root_grid) {
                LoadingOverlay overlay_view = new LoadingOverlay();
                root_grid.Children.Add(overlay_view);

                CurrentOverlay = overlay_view;
            }
        }

        public static void RemoveLoadingOverlay(Layout layout) {
            if (layout is Grid root_grid) {
                for (int i = root_grid.Children.Count - 1; i >= 0; i--) {
                    if (root_grid.Children[i] is LoadingOverlay) {
                        root_grid.Children.RemoveAt(i);
                    }
                }
            }
        }
    }
}
